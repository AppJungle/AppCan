/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace AppCan.wpf.Application
{
    /// <summary>
    /// Finds the controls in the window passed to construct the object.
    /// Menu name default is "MainMenu"
    /// Toolbar name is "ToolbarTray"
    /// Statusbar name is "Statusbar"
    /// </summary>
    class WindowElements : AppCan.wpf.Application.IWindowElements
    {
        Window _window;
        string _menuName = "MainMenu";
        string _toolbarName = "ToolBarTray";
        string _statusbarName = "Statusbar";



        /// <summary>
        /// Default constructor... takes the window that we want to find elements on.
        /// </summary>
        /// <param name="window"></param>
        public WindowElements(Window window)
        {
            _window = window;

        }

        /// <summary>
        /// Constructor that allows redefining the control names too look for
        /// </summary>
        /// <param name="window">The window</param>
        /// <param name="menuName">The menu name</param>
        /// <param name="toolbarName">The toolbar name</param>
        /// <param name="statusbarName">The statusbar name</param>
        public WindowElements(Window window,string menuName,string toolbarName, string statusbarName) : this(window)
        {
            _menuName = menuName;
            _toolbarName = toolbarName;
            _statusbarName = statusbarName;

        }

        /// <summary>
        /// Returns the MainMenu or null
        /// </summary>
        public Menu MainMenu
        {
            get
            {
                if (_menuName==null ||  _window == null)
                    return null;

                Menu mainMenu = _window.FindName(_menuName) as Menu;

                return mainMenu;
            }

        }

        /// <summary>
        /// Returns the toolbar or Null
        /// </summary>
        public ToolBarTray ToolBar
        {
            get
            {
                if (_toolbarName == null || _window == null)
                    return null;

                ToolBarTray sb = _window.FindName(_toolbarName) as ToolBarTray;

                return sb;

            }

        }


        /// <summary>
        /// Returns the statusbar or null.
        /// </summary>
        public StatusBar StatusBar
        {
            get
            {
                if (_statusbarName == null || _window==null)
                    return null;

                StatusBar sb = _window.FindName(_statusbarName) as StatusBar;

                return sb;

            }

        }

        /// <summary>
        /// Returns the window passed into the object
        /// </summary>
        public Window Window { get { return _window; }   }



    }
}
