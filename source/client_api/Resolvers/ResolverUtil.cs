#region Imports (8)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using YaTools.Yaml;

#endregion Imports (8)

namespace Craftitude.ClientApi.Resolvers
{


    public class ResolverUtil
    {
        public static ResolverBase GetResolverByName(string name)
        {
            var a = (from type in Assembly.GetExecutingAssembly().GetTypes()
                     where type.IsSubclassOf(typeof(ResolverBase)) && ((ResolverNameAttribute)type.GetCustomAttributes(typeof(ResolverNameAttribute), false).First()).Name.Equals(name)
                     select type).First();
            return (ResolverBase)Activator.CreateInstance(a);
        }
    }
}
