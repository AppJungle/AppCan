/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using AppCan.Core.Contexts;

namespace AppCan.wpf.Views
{
    /// <summary>
    /// Optional to use a a base class for your views that implements IView interface
    /// 
    /// </summary>
    public class ViewBase : UserControl,IView
    {
        protected IContext _context;
       

        public ViewBase()
        {


        }

        public ViewBase(IContext context)
        {
            _context = context;

        }

        public Core.Contexts.IContext Context
        {
            get
            {
                return _context;
            }
            set
            {
                _context=value;
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
                DataContext=value;
            }
        }

        public object DataContext
        {
            get
            {
                return DataContext;
            }
            set
            {
                DataContext=value;
            }
        }
    }
}
