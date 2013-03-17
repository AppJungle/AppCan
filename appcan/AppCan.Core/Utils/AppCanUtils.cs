/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace AppCan.Core.Utils
{
    /// <summary>
    /// This class contains static methods for utility functionality
    /// </summary>
    public static class AppCanUtils
    {
        public static bool FindRegisteredType(IUnityContainer container, Type typeToFind)
        {
            IEnumerable<ContainerRegistration> registered = container.Registrations;

            foreach (ContainerRegistration cr in registered)
            {
                if (cr.RegisteredType == typeToFind)
                    return true;

            }

            return false;

        }
    }
}
