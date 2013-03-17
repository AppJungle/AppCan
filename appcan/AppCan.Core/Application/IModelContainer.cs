/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppCan.Core.Application
{
    /// <summary>
    /// Model containers, contain a Model.
    /// </summary>
    public interface IModelContainer
    {
        /// <summary>
        /// The interace for the mode
        /// </summary>
        IModel Model { get; }

        /// <summary>
        /// The name of the file associated with this model - Null if no file name yet
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// The model template related to this model
        /// </summary>
        ModelTemplate Template { get; set; }
    }
}
