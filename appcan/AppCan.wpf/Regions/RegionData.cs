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
    /// Used by the ContextDefRegionManager
    /// </summary>
    public class RegionData
    {
        internal RegionData(string regionName, Type regionType, string context)
        {
            RegionName = regionName;
            RegionType = regionType;
            Context = context;
        }

        /// <summary>
        /// The region name
        /// </summary>
        public string RegionName;

        /// <summary>
        /// The region type
        /// </summary>
        public Type RegionType;

        /// <summary>
        /// The context name
        /// </summary>
        public string Context;
    }
}
