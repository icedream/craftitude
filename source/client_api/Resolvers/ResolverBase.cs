using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YaTools.Yaml;

namespace Craftitude.ClientApi.Resolvers
{
    public class ResolverBase : IResolver
    {
        private static string GetPlatformString()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.MacOSX:
                    return "osx";
                case PlatformID.Unix:
                    return "linux";
                case PlatformID.Xbox:
                    return "xbox";
                default:
                    return "windows";
            }
        }

        public object Input { get; set; }

        public virtual string ResolveToString()
        {
            throw new NotImplementedException();
        }

        public virtual ArchiveBase ResolveToArchive()
        {
            throw new NotImplementedException();
        }

        public virtual MemoryMappedFile ResolveToMemoryMappedFile()
        {
            throw new NotImplementedException();
        }

        public virtual Stream ResolveToStream()
        {
            throw new NotImplementedException();
        }

        public string Name
        { get { return ((ResolverNameAttribute)GetType().GetCustomAttributes(typeof(ResolverNameAttribute), false).First()).Name; } }

        public Hashtable Parameters
        { get; set; }
    }
}
