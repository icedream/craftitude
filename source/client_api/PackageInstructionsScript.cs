#region Imports (7)

using LuaInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#endregion Imports (7)

namespace Craftitude.ClientApi
{
    using Archives;
    using Resolvers;

    class PackageInstructionsScript : Lua
    {
        internal PackageMetadata _packageMetadata;
        internal ArchiveBase _sourcePackage;

        public PackageInstructionsScript()
        {

        }

        private void DeleteDir(string targetfile)
        {
            // TODO: Implement DeleteDir
        }

        private void DeleteFile(string targetfile)
        {
            // TODO: Implement DeleteFile
        }

        private object GetPackageMetadata(string propertyname)
        {
            return _packageMetadata.Data[propertyname];
        }

        private object GetPackageVersion()
        {
            // TODO: Implement GetPackageVersion
            throw new NotImplementedException();
        }

        private ResolverBase GetResolver(string name)
        {
            return GetResolver(name, new Hashtable());
        }

        private ResolverBase GetResolver(string name, Hashtable parameters)
        {
            var resolver = ResolverUtil.GetResolverByName(name);
            resolver.Parameters = parameters;
            return resolver;
        }

        private void InjectAllFromSource(string targetfile)
        {
            // TODO: Implement InjectAllFromSource
        }

        private void InjectAllFromSource(string targetfile, string filter)
        {
            // TODO: Implement UnpackAllFromSource with filter
        }

        private void Move(string source, string target)
        {
            // TODO: Implement Move
        }

        private void Rename(string source, string target)
        {
            Move(source, target);
        }

        private void UnpackAllFromSource(string targetdir)
        {
            _sourcePackage.ExtractAllFiles(targetdir);
        }

        private void UnpackAllFromSource(string targetdir, string filter)
        {
            // TODO: Implement UnpackAllFromSource with filter
        }
    }
}
