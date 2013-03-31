#region Imports (6)

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion Imports (6)

namespace Craftitude.ClientApi.Archives
{


    public abstract class ArchiveBase
    {
        public virtual void ExtractAllFiles(string targetFolderPath)
        {
            foreach (string fileEntry in GetFileEntries())
            {
                ExtractFile(fileEntry, Path.Combine(targetFolderPath, fileEntry.Replace('/', Path.DirectorySeparatorChar)));
            }
        }

        public virtual void ExtractFile(string file, string targetFilePath)
        {
            throw new NotImplementedException();
        }

        public virtual string[] GetFileEntries()
        {
            throw new NotImplementedException();
        }

        public virtual Stream OpenFile(string file)
        {
            throw new NotImplementedException();
        }
    }
}
