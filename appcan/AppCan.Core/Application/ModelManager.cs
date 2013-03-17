/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using RCLibrary;
using System.Runtime.Serialization;

namespace AppCan.Core.Application
{
    /// <summary>
    /// Event arguments for model events.  Properties are set based on the event that is called
    /// </summary>
    public class ModelEventArgs : EventArgs
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ModelEventArgs() : this(null,null,null)
        {
           
        }

        public ModelEventArgs(ModelTemplate mt, IModelContainer mc, string fileName)
        {
            Template = mt;
            ModelContainer = mc;
            FileName = fileName;
            AbortAction = false;
        }

        /// <summary>
        /// The template or Null
        /// </summary>
        public ModelTemplate Template { get; internal set; }

        /// <summary>
        /// The model container or null
        /// </summary>
        public IModelContainer ModelContainer { get; internal set; }

        /// <summary>
        /// The filename or null
        /// </summary>
        public string FileName { get; internal set; }

        /// <summary>
        /// Set to false by default.  For certain events can abort an action by setting to true
        /// </summary>
        public bool AbortAction { get; set; }
    }

    public delegate void ModelEventHandler(object sender, ModelEventArgs e);

    /// <summary>
    /// Keeps track of ModelTemplate's and ModelContainers
    /// </summary>
    public class ModelManager : AppCan.Core.Application.IModelManager
    {
        List<ModelTemplate> _templates = new List<ModelTemplate>();
        RCLinkedList<ModelContainer> _modelContainers = new RCLinkedList<ModelContainer>();

        public List<ModelTemplate> Templates { get { return _templates; } }

        public ModelContainer FirstModelContainer { get { return _modelContainers.First != null ? _modelContainers.First.Value : null; } }

        /// <summary>
        /// Fired when a ModelContainer is about to be opened.  The FileName is set on the ModelEventArgs.
        /// AbortAction if set to true will prevent the ModelContainer from being opened.
        /// </summary>
        public event ModelEventHandler Opening;

        /// <summary>
        /// Fired after a ModelContainer has been opened.  The ModelContainer property will be set on the ModelEventArgs.
        /// AbortAction has no effect
        /// </summary>
        public event ModelEventHandler Opened;

        /// <summary>
        /// Fired before closing a ModelContainer.  The ModelContainer property of the ModelEventArgs will be set.  AbortAction will prevent 
        /// the ModelContaienr from being closed.
        /// </summary>
        public event ModelEventHandler Closing;

        /// <summary>
        /// Fired when a ModelContainer has been closed.  The ModelContainer property of the ModelEventArgs  will be set.  
        /// AbortAction has no effect.
        /// </summary>
        public event ModelEventHandler Closed;

        /// <summary>
        /// Add a Model template to the available list
        /// </summary>
        /// <param name="template"></param>
        public virtual void AddModelTemplate(ModelTemplate template)
        {
            _templates.Add(template);
        }

        /// <summary>
        /// Remove a specific model template from the available list
        /// </summary>
        /// <param name="template">The template to be removed</param>
        /// <returns>true if removed, false otherwise</returns>
        public virtual bool RemoveModelTemplate(ModelTemplate template)
        {
            return _templates.Remove(template);
        }

        /// <summary>
        /// Find a model template by the file extension used.
        /// </summary>
        /// <param name="fileExtension">The file extension to search for</param>
        /// <returns>The model template or null if none found</returns>
        public virtual ModelTemplate FindModelTemplate(string fileExtension)
        {
            for (int x = 0; x < _templates.Count; x++)
            {
                if (_templates[x].FileExtension == fileExtension)
                    return _templates[x];

            }

            return null;

        }

        /// <summary>
        /// Open a ModelContainer by the filename
        /// </summary>
        /// <param name="fileName">The filename to open</param>
        /// <returns>The new Model Container that was loaded, or existing one if the file is already open, or null if could not be opened.</returns>
        public virtual ModelContainer OpenModelContainer(string fileName)
        {
            if (fileName == null)
                return null;

            //see if document already exists
            ModelContainer foundDoc=_modelContainers.Find(d => d.FileName == fileName);
            if (foundDoc != null)
                return foundDoc;

            string extension=Path.GetExtension(fileName);

            if (extension == null)
                return null;


            ModelTemplate template=FindModelTemplate(extension);

            if (template == null)
                return null;

            ModelEventArgs mea = new ModelEventArgs(null, null, fileName);
            //Fire event that we are about to open a file
            OnOpening(mea);

            //TODO: Handle AbortAction better
            if (mea.AbortAction==true)
                return null;  //a listener on the event wanted to abort the action

            ModelContainer doc = new ModelContainer();




            doc.Model = LoadModelFromFile(template.ModelType, fileName,template.SerializerAdapter);
            doc.FileName = fileName;
            doc.Template = template;

            _modelContainers.AddFirst(doc);

            ///Fire event that we opened the modelcontainer
            OnOpened(new ModelEventArgs(null, doc, null));

            return doc;
        }

        /// <summary>
        /// Allow saving of a particular document
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public bool SaveModelContainer(ModelContainer doc)
        {
            
            return SaveModelToFile(doc.Template.ModelType, doc.Model, doc.Template.SerializerAdapter, doc.FileName);

        }

        //TODO: ModelManager needs a close method, model also needs a dirty flag and a way to save or save as to be implemented


        /// <summary>
        /// Called to Close a ModelContainer
        /// </summary>
        /// <param name="mc"></param>
        /// <returns></returns>
        public bool CloseModelContainer(IModelContainer mc)
        {
            //Notify closing
            ModelEventArgs mea = new ModelEventArgs(null, mc, null);
            OnClosing(mea);
            //TODO: Handle abort action better
            if (mea.AbortAction == true)
                return false;  //event listener requested to abort the action

            bool ret=_modelContainers.Remove((ModelContainer)mc);

            OnClosed(new ModelEventArgs(null, mc, null));

            return ret;
        } 

        /// <summary>
        /// Protected method to load the model from the file
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected IModel LoadModelFromFile(Type model,string fileName, ISerializerAdapter serializerAdapter)
        {
            IModel returnVal = null;

            if (!File.Exists(fileName))
            {
                return null;

            }


            if (model is ISerializable)
            {
                returnVal = (IModel)serializerAdapter.DeserializeObject(fileName, model);                    //Activator.CreateInstance(model);

                return returnVal;
            }
            else
            {

                return null;
            }

           
        }

        /// <summary>
        /// protected class method to save a model to a file
        /// </summary>
        /// <param name="modelType"></param>
        /// <param name="model"></param>
        /// <param name="serializerAdapter"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected bool SaveModelToFile(Type modelType, IModel model, ISerializerAdapter serializerAdapter,string fileName)
        {
            if (model is ISerializable)
            {

                return serializerAdapter.SerializeObject(fileName, model, modelType);
                
            }
            else {

                return false;
            }

        }


        /// <summary>
        /// Protected method to create a new model container from a template
        /// </summary>
        /// <param name="template">The template to use to create the model container</param>
        /// <returns>The new model container</returns>
        protected ModelContainer NewModelContainer(ModelTemplate template)
        {
            ModelContainer doc = new ModelContainer();


            doc.Model = (IModel)Activator.CreateInstance(template.ModelType); //LoadModelFromFile(template.ModelType, fileName, template.SerializerAdapter);
            doc.FileName = GetUniqueFileName(template.FilePattern);
            doc.Template = template;

            _modelContainers.AddFirst(doc);

            return doc;

        }

        /// <summary>
        /// Creates a new default blank modelcontainer
        /// </summary>
        /// <returns>The new default modelcontainer</returns>
        public ModelContainer NewDefaultBlankModelContainer()
        {
            return NewModelContainer(GetDefaultTemplate());

        }

        /// <summary>
        /// Gets the default configured template
        /// </summary>
        /// <returns></returns>
        public ModelTemplate GetDefaultTemplate()
        {
            return _templates.Find(d => d.IsDefaultTemplate == true);

        }


        /// <summary>
        /// Based on the base file name, returns a unique filename if it is able to within a certain number of attempts
        /// </summary>
        /// <param name="filePattern"></param>
        /// <returns>returns the attempted unique filename</returns>
        protected string GetUniqueFileName(string filePattern)
        {
            int x = 1;
            string file=string.Format(filePattern,x);
            
            while (File.Exists(file) && x<100)
            {
                x++;
                file = string.Format(filePattern, x);
            }

            return file;

        }

        /// <summary>
        /// Call this before opening a model container
        /// </summary>
        /// <param name="e">Pass the filename that will be opened</param>
        protected virtual void OnOpening(ModelEventArgs e)
        {
            ModelEventHandler meh = Opening;
            if (meh != null)
            {
                meh(null, e);

            }

        }

        /// <summary>
        /// Call this after opening a modelcontainer
        /// </summary>
        /// <param name="e">Pass the ModelContainer that was opened</param>
        protected virtual void OnOpened(ModelEventArgs e)
        {
            ModelEventHandler meh = Opened;
            if (meh != null)
            {
                meh(null, e);

            }

        }

        /// <summary>
        /// Call just before closing a model container
        /// </summary>
        /// <param name="e">Pass the ModelContainer that is being closed</param>
        protected virtual void OnClosing(ModelEventArgs e)
        {
            ModelEventHandler meh = Closing;
            if (meh != null)
            {
                meh(null, e);

            }

        }

        /// <summary>
        /// Call after closing a modelcontainer
        /// </summary>
        /// <param name="e">Pass the ModelContainer that was closed or the filename</param>
        protected virtual void OnClosed(ModelEventArgs e)
        {
            ModelEventHandler meh = Closed;
            if (meh != null)
            {
                meh(null, e);

            }

        }


        

    }
}
