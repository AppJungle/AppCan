/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppCan.wpf.Regions
{
    /// <summary>
    /// This service is used by ContextDef's to store Region information that should be propogated to contexts when they are created.
    /// The WpfContextController is resoponsible for pulling the regions out of this service from the context and initializing the region information
    /// into the newly created context.
    /// </summary>
    public interface IContextRegionManager
    {
        /// <summary>
        /// The region name to register to
        /// </summary>
        /// <typeparam name="T">The type that should be instatiated for this region</typeparam>
        /// <param name="regionName">The region name</param>
        void RegisterViewWithRegion<T>(string regionName);

        /// <summary>
        /// The type to register with a region and the context it should be created from.
        /// </summary>
        /// <typeparam name="T">The type to instatiate for this region</typeparam>
        /// <param name="regionName">The name of the region</param>
        /// <param name="context">When the region requests the view is created a built in delegate is called that creates the context and resolves the IView or the IViewModel based on the WpfCreationOp that is configured for the context definition.  ViewFirst creates IView, ViewModelFirst creates IViewModel</param>
        void RegisterViewWithRegion<T>(string regionName, string context);

        /// <summary>
        /// The type to register with a region
        /// </summary>
        /// <param name="regionName">The region name</param>
        /// <param name="type">The type to instatiate</param>
        void RegisterViewWithRegion(string regionName, Type type);

        /// <summary>
        /// The type to register with a region and the context it should be created from.
        /// </summary>
        /// <param name="regionName">The region name</param>
        /// <param name="type">The type</param>
        /// <param name="context">The context that will be used to instatiate the type from</param>
        void RegisterViewWithRegion(string regionName, Type type, string context);

        /// <summary>
        /// The list of RegionData that have region information
        /// </summary>
        List<RegionData> Regions { get; }
    }
}
