/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;


namespace AppCan.Core.Application
{
    /// <summary>
    /// This interface is for use by models that will be derived from by a specific class to store information for the application.
    /// </summary>
    public interface IModel : INotifyPropertyChanged
    {
    }
}
