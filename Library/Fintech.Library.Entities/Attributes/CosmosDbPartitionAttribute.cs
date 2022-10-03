using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.Library.Entities.Attributes
{
    public class CosmosDbConfigAttribute : Attribute
    {
        public string ContainerName { get; set; }
        public CosmosDbConfigAttribute(string containerName)
        {
            ContainerName = containerName;
        }
    }
}
