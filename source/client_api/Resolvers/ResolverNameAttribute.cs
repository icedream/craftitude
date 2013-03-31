using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craftitude.ClientApi.Resolvers
{
    public class ResolverNameAttribute : Attribute
    {
        public string Name { get; set; }

        public ResolverNameAttribute(string name)
        {
            this.Name = name;
        }
    }
}
