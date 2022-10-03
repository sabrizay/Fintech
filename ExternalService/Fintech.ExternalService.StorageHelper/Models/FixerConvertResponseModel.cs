using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.ExternalService.StorageHelper.Models
{
    public class FixerConvertResponseModel
    {
        public bool success { get; set; }
        public Query query { get; set; }
        public Info info { get; set; }
        public string date { get; set; }
        public decimal result { get; set; }
    }

    public class Info
    {
        public int timestamp { get; set; }
        public double rate { get; set; }
    }

    public class Query
    {
        public string from { get; set; }
        public string to { get; set; }
        public int amount { get; set; }
    }
}
