/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
namespace AppCan.Core.Contexts
{
    /// <summary>
    /// The interface for the context manager
    /// </summary>
    public interface IContextManager
    {
        //Context CreateOrActivateContext(string contextName);

        /// <summary>
        /// Get's a new context
        /// </summary>
        /// <param name="contextName">The context name</param>
        /// <returns>The context</returns>
        Context GetContext(string contextName);

        /// <summary>
        /// Get a new or existing context definition
        /// </summary>
        /// <param name="contextName">The context name</param>
        /// <returns>Context definition</returns>
        ContextDef GetNewOrExistingContextDef(string contextName);
    }
}
