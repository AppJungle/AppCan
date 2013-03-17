using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AppCan.wpf.Views;
using AppCan.Core.Contexts;

namespace HelloWorldModule.Views
{
    /// <summary>
    /// Interaction logic for ToolbarView.xaml
    /// </summary>
    public partial class ToolbarView : UserControl, IView
    {
        private IContext _context;

        public ToolbarView(IContext context)
        {
            _context = context;
            InitializeComponent();
        }

        public AppCan.Core.Contexts.IContext Context
        {
            get
            {
                return _context;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IViewModel ViewModel
        {
            get
            {
                return DataContext as IViewModel;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
