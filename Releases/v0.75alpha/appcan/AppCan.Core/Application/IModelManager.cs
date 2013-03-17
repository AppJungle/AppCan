/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
namespace AppCan.Core.Application
{

    /// <summary>
    /// The interface to the Model Manager that keeps track of Model templates and model containers
    /// </summary>
    public interface IModelManager
    {
        /// <summary>
        /// Add a model template to the model manager
        /// </summary>
        /// <param name="template">The model template to add</param>
        void AddModelTemplate(ModelTemplate template);

        /// <summary>
        /// Find a model template by file extension
        /// </summary>
        /// <param name="fileExtension">The file extension to search for</param>
        /// <returns></returns>
        ModelTemplate FindModelTemplate(string fileExtension);

        /// <summary>
        /// The first ModelContainer in the list 
        /// </summary>
        ModelContainer FirstModelContainer { get; }


        /// <summary>
        /// Get the default template
        /// </summary>
        /// <returns>The default template</returns>
        ModelTemplate GetDefaultTemplate();

        /// <summary>
        /// Get a default blank Model container
        /// </summary>
        /// <returns></returns>
        ModelContainer NewDefaultBlankModelContainer();

        /// <summary>
        /// Open a model container by file name
        /// </summary>
        /// <param name="fileName">The file name to open</param>
        /// <returns>The opened model container</returns>
        ModelContainer OpenModelContainer(string fileName);

        /// <summary>
        /// Remove a model template
        /// </summary>
        /// <param name="template">The template to remove</param>
        /// <returns>true if removed otherwise false</returns>
        bool RemoveModelTemplate(ModelTemplate template);

        /// <summary>
        /// Saves the model container
        /// </summary>
        /// <param name="doc">The model container to save</param>
        /// <returns>true if saved, otherwise false</returns>
        bool SaveModelContainer(ModelContainer doc);

        /// <summary>
        /// The list of Model Templates registered with the model manager
        /// </summary>
        System.Collections.Generic.List<ModelTemplate> Templates { get; }

        /// <summary>
        /// Close the Model Container
        /// </summary>
        /// <param name="mc">The model container</param>
        /// <returns>true if closed, false otherwise</returns>
        bool CloseModelContainer(IModelContainer mc);

        /// <summary>
        /// Fired when a ModelContainer is about to be opened.  The FileName is set on the ModelEventArgs.
        /// AbortAction if set to true will prevent the ModelContainer from being opened.
        /// </summary>
        event ModelEventHandler Opening;

        /// <summary>
        /// Fired after a ModelContainer has been opened.  The ModelContainer property will be set on the ModelEventArgs.
        /// AbortAction has no effect
        /// </summary>
        event ModelEventHandler Opened;

        /// <summary>
        /// Fired before closing a ModelContainer.  The ModelContainer property of the ModelEventArgs will be set.  AbortAction will prevent 
        /// the ModelContaienr from being closed.
        /// </summary>
        event ModelEventHandler Closing;

        /// <summary>
        /// Fired when a ModelContainer has been closed.  The ModelContainer property of the ModelEventArgs  will be set.  
        /// AbortAction has no effect.
        /// </summary>
        event ModelEventHandler Closed;
    }
}
