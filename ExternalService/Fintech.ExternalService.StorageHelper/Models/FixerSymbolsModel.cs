using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.ExternalService.StorageHelper.Models
{
    public class FixerSymbolsModel
    {
        public bool success { get; set; } = false;
       
        public List<rates> rates { get; set; }
        public FixerSymbolsModel()
        {
            rates = new List<rates>();
        }
    }


    public class rates
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }



}
