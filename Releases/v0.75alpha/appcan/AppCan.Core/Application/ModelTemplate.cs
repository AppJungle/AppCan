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
    /// This class creates a relationship between file types, Model implementations, and code available to display them.
    /// </summary>
    public class ModelTemplate
    {
        private Type _modelType;
        private string _fileExtension;
        ISerializerAdapter _serializerAdapter;
        bool _isDefaultTemplate;
        Type _startupWindowType;
        string _startupUri;
        string _filePattern;
        string _contextName;


        /// <summary>
        /// Constructor for a model template
        /// </summary>
        /// <param name="modelType">The type for the Model class</param>
        /// <param name="fileExtenstion">The file extension to associate</param>
        /// <param name="filePattern">A default file pattern to use</param>
        /// <param name="serializerAdapter">The serialization adapter to use</param>
        /// <param name="isDefaultTemplate">true if the default template, false otherwise... there should be only one default template</param>
        /// <param name="startupWindowType">null</param>
        /// <param name="startupUri">null</param>
        /// <param name="contextName">The context name that contains the shell IViewContainer to start</param>
        public ModelTemplate(Type modelType, string fileExtenstion,string filePattern, ISerializerAdapter serializerAdapter,
            bool isDefaultTemplate,Type startupWindowType, string startupUri,string contextName)
        {
            _modelType = modelType;
            _fileExtension = fileExtenstion;
            _filePattern = filePattern;
            _serializerAdapter = serializerAdapter;
            _isDefaultTemplate = isDefaultTemplate;
            _startupWindowType = startupWindowType;
            _startupUri = startupUri;
            _contextName = contextName;
        }

        

        /// <summary>
        /// The serialization adapter
        /// </summary>
        public ISerializerAdapter SerializerAdapter
        {
            get
            {
                return _serializerAdapter;

            }

        }

        /// <summary>
        /// The model type
        /// </summary>
        public Type ModelType { get { return _modelType; } }

        /// <summary>
        /// The file extension
        /// </summary>
        public string FileExtension { get { return _fileExtension; } }

        /// <summary>
        /// True if the default template, false otherwise
        /// </summary>
        public bool IsDefaultTemplate { get { return _isDefaultTemplate; } }

        /// <summary>
        /// null
        /// </summary>
        public Type StartupWindowType { get { return _startupWindowType; } }
        
        /// <summary>
        /// null
        /// </summary>
        public string StartupUri { get { return _startupUri; } }

        /// <summary>
        /// The file pattern
        /// </summary>
        public string FilePattern { get { return _filePattern; } }

        /// <summary>
        /// The context name
        /// </summary>
        public string ContextName { get { return _contextName; } }

    }
}
