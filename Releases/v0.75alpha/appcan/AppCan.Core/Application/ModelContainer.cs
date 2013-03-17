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
    /// Model container class contains the model with the data.
    /// </summary>
    public class ModelContainer : IModelContainer
    {
        /// <summary>
        /// The file associated with this model
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The model/data
        /// </summary>

        public IModel Model { get; set; }

        /// <summary>
        /// The template associated with this model
        /// </summary>
        public ModelTemplate Template { get; set; }
    }
}
