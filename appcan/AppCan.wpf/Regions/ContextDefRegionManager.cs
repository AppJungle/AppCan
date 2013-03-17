/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppCan.Core.Contexts;

namespace AppCan.wpf.Regions
{
    /// <summary>
    /// This service is used by ContextDef's to store Region information that should be propogated to contexts when they are created.
    /// The WpfContextController is resoponsible for pulling the regions out of this service from the context and initializing the region information
    /// into the newly created context.
    /// </summary>
    class ContextDefRegionManager : IContextRegionManager
    {
        
        //private IContext _context;
        private List<RegionData> _regions = new List<RegionData>();

        /// <summary>
        /// The list of regions to propogate to the context
        /// </summary>
        public List<RegionData> Regions { get { return _regions; } }

        public ContextDefRegionManager()
        {
            

        }

        /// <summary>
        /// Register the view T with the regionName
        /// </summary>
        /// <typeparam name="T">The view type to register</typeparam>
        /// <param name="regionName">The region name to register to</param>
        public void RegisterViewWithRegion<T>(string regionName)
        {
            _regions.Add(new RegionData(regionName,typeof(T),null));

        }

        /// <summary>
        /// Register view with Region T to regionNam in the context
        /// </summary>
        /// <typeparam name="T">The view type to register</typeparam>
        /// <param name="regionName">The region name to register to</param>
        /// <param name="context">The context that contains the type T and any dependent types</param>
        public void RegisterViewWithRegion<T>(string regionName,string context)
        {
            _regions.Add(new RegionData(regionName, typeof(T), context));

        }

        /// <summary>
        /// Register the view with region
        /// </summary>
        /// <param name="regionName">The region name to register to</param>
        /// <param name="type">The type to register for this region</param>
        public void RegisterViewWithRegion(string regionName,Type type)
        {
            _regions.Add(new RegionData(regionName, type,null));

        }

        /// <summary>
        /// Register the view with the region and create the type from the specified context.
        /// </summary>
        /// <param name="regionName">The region name to register to</param>
        /// <param name="type">The type of view to add to the region</param>
        /// <param name="context">The context that has the container that has the type registered in it and any dependent types</param>
        public void RegisterViewWithRegion(string regionName, Type type, string context)
        {
            _regions.Add(new RegionData(regionName, type, context));

        }

    }
}
