/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace AppCan.Core.Component
{
    /// <summary>
    /// For classes that need to implement INotifyPropertyChanged, can derive from this base class and call InvokePropertyChanged with the property name.
    /// </summary>
    public class BaseNotifyPropertyChanged : INotifyPropertyChanged
    {
        /// <summary>
        /// The property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Call this when a property changes
        /// </summary>
        /// <param name="propertyName">The name of the property that changed</param>
        protected virtual void InvokePropertyChanged(string propertyName)
        {
            var e = new PropertyChangedEventArgs(propertyName);
            PropertyChangedEventHandler changed = PropertyChanged;
            if (changed != null) changed(this, e);
        }
    }
}
