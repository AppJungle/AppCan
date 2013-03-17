/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppCan.Core.Contexts;
using AppCan.wpf.Views;
using Microsoft.Practices.Unity;

namespace AppCan.wpf.Views
{
    public class ViewModelExtensionsController : IViewModelExtensionsController
    {
        public bool PopulateAllViewModels { get; set; }

        public ViewModelExtensionsController()
        {
            PopulateAllViewModels = true;

        }

        /// <summary>
        /// Find and attach any IViewModelExtensions to viewmodels if PopulateAllViewModels is true;
        /// </summary>
        /// <param name="context"></param>
        public void FindAndAttachViewExtensions(IContext context)
        {
            if (!PopulateAllViewModels)
                return; //nothing to do no population required

            bool hasViewModels = false;
            IEnumerable<IViewModelExtension> viewModelExtensions = context.Container.ResolveAll<IViewModelExtension>();
            if (!viewModelExtensions.Any())
                return;   //nothing to attach

            IViewModel viewModel = null;
            if (FindRegisteredType(context, typeof(IViewModel)))
            {
                viewModel = context.Container.Resolve<IViewModel>();
                hasViewModels = true;
            }

            //get named view models
            IEnumerable<IViewModel> viewModels=context.Container.ResolveAll<IViewModel>();

            if (viewModel==null && !viewModels.Any())
                return; //no view models to attach anything to

            //Allocate extension dictionaries if they don't already exist
            if (viewModel != null)
            {
                if (viewModel.Extensions == null)
                {
                    viewModel.Extensions = new Dictionary<string, IViewModelExtension>();
                }
            }

            //Initially any named view models
            foreach (IViewModel vm in viewModels)
            {
                if (vm.Extensions == null)
                {
                    vm.Extensions = new Dictionary<string, IViewModelExtension>();
                }

            }

            //for each viewmodel extension add it to all of the view models named and unnamed
            foreach(IViewModelExtension vme in viewModelExtensions)
            {
                if (viewModel!=null)
                {
                    viewModel.Extensions[vme.ExtensionName] = vme;    
                }

                foreach (IViewModel vm in viewModels)
                {
                    vm.Extensions[vme.ExtensionName] = vme;

                }
    
            }


        }

        protected bool FindRegisteredType(IContext context, Type typeToFind)
        {
            IEnumerable<ContainerRegistration> registered = context.Container.Registrations;

            foreach (ContainerRegistration cr in registered)
            {
                if (cr.RegisteredType == typeToFind)
                    return true;

            }

            return false;

        }
    }
}
