/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
namespace AppCan.wpf.Application
{
    /// <summary>
    /// Finds the controls in the window passed to construct the object.
    /// </summary>
    public interface IWindowElements
    {
        /// <summary>
        /// Returns the MainMenu or null
        /// </summary>
        System.Windows.Controls.Menu MainMenu { get; }

        /// <summary>
        /// Returns the statusbar or null.
        /// </summary>
        System.Windows.Controls.Primitives.StatusBar StatusBar { get; }


        /// <summary>
        /// Returns the toolbar or Null
        /// </summary>
        System.Windows.Controls.ToolBarTray ToolBar { get; }

        /// <summary>
        /// Returns the window passed into the object
        /// </summary>
        System.Windows.Window Window { get; }
    }
}
