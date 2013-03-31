using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craftitude.ClientApi
{
    using Archives;

    namespace Resolvers
    {
        [ResolverName("archive")]
        public class ArchiveResolver : ResolverBase
        {
            public override ArchiveBase ResolveToArchive()
            {
                if (!this.Parameters["Input"].GetType().IsSubclassOf(typeof(Stream)))
                    throw new InvalidOperationException();

                string password = null;
                if (Parameters.ContainsKey("password") && !string.IsNullOrEmpty(Parameters["password"].ToString()))
                    password = Parameters["password"].ToString();

                var archive = new Archive(((Stream)this.Parameters["Input"]), password);

                return archive;
            }
        }
    }

}