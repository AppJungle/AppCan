/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using AppCan.wpf.Application;
using AppCan.Core.Application;
using System.Windows.Input;
using System.Windows.Navigation;
using AppCan.wpf.Views;
using AppCan.Core.Contexts;
using Microsoft.Practices.Unity;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Win32;


namespace AppCan.wpf.Application
{
    /// <summary>
    /// Base application class for AppCan Wpf applications
    /// Also need to change the xaml in the App.xaml as well as the base class for the App
    /// example of specifying alternative namespaces xmlns:y="clr-namespace:AppCanSample;assembly=AppCanSample"
    /// </summary>
    public class AppCanApp : System.Windows.Application
    {
        static IModelManager _docManager;
        static object _singletonLock = new object();

        bool initializedViewModel = false;
        IViewModel _mainInitialViewModel = null;
        ModelContainer _mainInitialDocument = null;

        CommandBinding _openBinding;
        CommandBinding _saveBinding;
        CommandBinding _exitBinding;
        CommandBinding _saveasBinding;

        ContextManager _contextManager;

        IUnityContainer _container;

        string[] _args;

        /// <summary>
        /// The Arguments the application was started with
        /// </summary>
        protected string[] Args { get { return _args; } }

        
        protected string DefaultContextName { get; set; }

        /// <summary>
        /// Gets the root unity container (or derived classes can set it)
        /// </summary>
        public IUnityContainer Container { get { return _container; } protected set { _container = value; } }

        /// <summary>
        /// Returns context manager (or derived class can set the context manager)
        /// </summary>
        public ContextManager ContextManager { get { return _contextManager; } protected set { _contextManager = value; } }

        /// <summary>
        /// Returns the context instance used for the shell main window 
        /// </summary>
        public IContext ShellContext
        {
            get
            {
                IViewContainer vc = MainWindow as IViewContainer;
                if (vc != null)
                {
                    return vc.Context;
                }
                return null;
            }

        }


        /// <summary>
        /// Default constructor
        /// </summary>
        public AppCanApp()
        {
            _openBinding = new CommandBinding(ApplicationCommands.Open, OpenCommand, CanExecuteOpenCommand);
            _saveBinding = new CommandBinding(ApplicationCommands.Save, SaveCommand, CanExecuteSaveCommand);
            _exitBinding = new CommandBinding(AppCan.wpf.Application.AppCanCommands.Exit, ExitCommand, CanExecuteExitCommand);
            _saveasBinding = new CommandBinding(ApplicationCommands.SaveAs, SaveAsCommand, CanExecuteSaveAsCommand);

            // Register CommandBinding for all windows. 
            CommandManager.RegisterClassCommandBinding(typeof(Window), _openBinding);
            CommandManager.RegisterClassCommandBinding(typeof(Window), _saveBinding);
            CommandManager.RegisterClassCommandBinding(typeof(Window), _exitBinding);
            CommandManager.RegisterClassCommandBinding(typeof(Window), _saveasBinding);

            Activated += new EventHandler(AppCanApp_Activated);

        }

        /// <summary>
        /// Should be called from the BootStrapper when createshell is called
        /// </summary>
        /// <param name="shell"></param>
        /// <returns></returns>
        public virtual DependencyObject CreateShell(Type shell)
        {
            var container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            Container = container;

            InitDefaultServices(container);

            PostInitialization();

            return null;

        }

        

        /// <summary>
        /// Should be called from the BootStrapper when InitialiazeShell is called
        /// </summary>
        public virtual void InitializeShell()
        {
            
        }

        /// <summary>
        /// Should be called from the BootStrapper when ConfigureModuleCatalog is called.
        /// </summary>
        public virtual void ConfigureModuleCatalog()
        {

        }

        /// <summary>
        /// Should be called from the BootStrapper when InitializeModules is called.
        /// </summary>
        public virtual void InitializeModules()
        {
            // Register the IWindowElements interface for the main window
            Container.RegisterInstance<IWindowElements>(new WindowElements(MainWindow));
            //PostInitialization();

        }


        /// <summary>
        /// Called internally to initialize many of the default managers/services/controllers
        /// </summary>
        /// <param name="container"></param>
        protected virtual void InitDefaultServices(IUnityContainer container)
        {
            _contextManager = new WpfContextManager(container, typeof(WpfContextController));
            container.RegisterType<IContextController, WpfContextController>();  //will create multiple instances when resolved

            container.RegisterInstance<IContextManager>(_contextManager);

            container.RegisterInstance<IModelManager>(ModelManager);
        }


        /// <summary>
        /// Called when the app is activated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AppCanApp_Activated(object sender, EventArgs e)
        {


            if (!initializedViewModel)
            {
                IView view = MainWindow as IView;
                if (view != null)
                {
                    if (view.ViewModel != null && _mainInitialDocument!=null)
                    {
                        //intialize the view mode
                        view.ViewModel.Model = _mainInitialDocument.Model;
                        view.ViewModel.ModelContainer = _mainInitialDocument;
                        view.ViewModel.View = view;

                        //set the view model to the view
                        view.DataContext = view.ViewModel;

                    }
                }

                initializedViewModel = true;
            }

            
        }

        /// <summary>
        /// Gets or sets the model manager.
        /// </summary>
        public static IModelManager ModelManager {
         get {
             if (_docManager==null)
                {
                    lock (_singletonLock)
                    {
                        if (_docManager==null)
                            _docManager=new ModelManager();

                        return _docManager;
                    }
                } else return _docManager;
            }

            set
            {

                lock (_singletonLock)
                {
                    _docManager = value;
                }
            }
        }

        /// <summary>
        /// Called when the application is started.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            
            _args = e.Args;
            base.OnStartup(e);
            
        }

        /// <summary>
        /// Used to initialize the default document and get the default context to use
        /// </summary>
        protected virtual void PostInitialization()
        {

            ModelManager.Opening += new ModelEventHandler(ModelManager_Opening);
            

            if (Args.Length == 0)
            {
                //new document
                ModelTemplate template = ModelManager.GetDefaultTemplate();
                if (template != null)
                {
                    DefaultContextName = template.ContextName;
                    _mainInitialDocument = ModelManager.NewDefaultBlankModelContainer();
                }
                else
                {
                    //WARNING no window defined.  Hope this is what you intended 
                }


            }
            else
            {
                ModelContainer doc = ModelManager.OpenModelContainer(Args[0]);

                if (doc != null)
                {
                    DefaultContextName = doc.Template.ContextName;
                    _mainInitialDocument = doc;
                }
                else
                { //blank doc
            
                    _mainInitialDocument = ModelManager.NewDefaultBlankModelContainer();
                    DefaultContextName = _mainInitialDocument.Template.ContextName;

                }
            }

        }

        /// <summary>
        /// When the model manager is opening a new file it calls this method.
        /// The default implementation closes the previously opened document 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void ModelManager_Opening(object sender, ModelEventArgs e)
        {
            //TODO: Add code for making this only happen in single document mode
            if (ModelManager.FirstModelContainer != null)
            {
                //TODO: Save document if needed.
                ModelManager.CloseModelContainer(ModelManager.FirstModelContainer);

            }
        }

        /// <summary>
        /// Called when the main window is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
        
            
        }

        /// <summary>
        /// Called when loading the app has completed
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoadCompleted(NavigationEventArgs e)
        {
            base.OnLoadCompleted(e);

            

        }

        /// <summary>
        /// Called when the application is activated
        /// </summary>
        /// <param name="e"></param>
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

        

        }


        /// <summary>
        /// Called on the application open command.  By default requests a model container to open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OpenCommand(object sender, ExecutedRoutedEventArgs e) 
        { 
            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog().Value)
            {
                AppCanApp.ModelManager.OpenModelContainer(openDialog.FileName);
            }
        }

        /// <summary>
        /// Called on the application open commands can execute call
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void CanExecuteOpenCommand(object sender, CanExecuteRoutedEventArgs e) 
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Called on the application save command.  Default tries to save the open modelcontainer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void SaveCommand(object sender, ExecutedRoutedEventArgs e)
        {
            

            AppCanApp.ModelManager.SaveModelContainer(ModelManager.FirstModelContainer);

        }

        /// <summary>
        /// Called on the Application Save can execute command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void CanExecuteSaveCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        /// <summary>
        /// Called on the AppCanCommand.Exit Command, causes the app to be shutdown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void ExitCommand(object sender, ExecutedRoutedEventArgs e)
        {
            this.Shutdown();
            
        }

        /// <summary>
        /// AppCanCommands.Exit Can Execute Command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void CanExecuteExitCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        /// <summary>
        /// Called on ApplicationCommand Save As - default saves the open model container, asking the file to save as...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void SaveAsCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (ModelManager.FirstModelContainer != null)
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                
                dlg.FileName = ModelManager.FirstModelContainer.FileName; // Default file name
                dlg.DefaultExt = ModelManager.FirstModelContainer.Template.FileExtension; // Default file extension
                dlg.Filter = "Documents (" + ModelManager.FirstModelContainer.Template.FileExtension + ")|*" + ModelManager.FirstModelContainer.Template.FileExtension; // Filter files by extension
                Nullable<bool> result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    // Save document
                    ModelManager.FirstModelContainer.FileName = dlg.FileName;
                    AppCanApp.ModelManager.SaveModelContainer(ModelManager.FirstModelContainer);
                }

                
            }

        }

        /// <summary>
        /// Application Save As command can execute
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void CanExecuteSaveAsCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

    }
}
