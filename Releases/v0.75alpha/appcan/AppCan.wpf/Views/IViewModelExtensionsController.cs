/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
namespace AppCan.wpf.Views
{
    public interface IViewModelExtensionsController
    {
        void FindAndAttachViewExtensions(AppCan.Core.Contexts.IContext context);
        bool PopulateAllViewModels { get; set; }
    }
}
