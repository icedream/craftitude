﻿#region Imports (7)

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion Imports (7)

namespace Craftitude.ClientApi
{
    using Archives;

    namespace Resolvers
    {
        interface IResolver
        {
            string ResolveToString();
            ArchiveBase ResolveToArchive();
            MemoryMappedFile ResolveToMemoryMappedFile();
            Stream ResolveToStream();
        }
    }
}
