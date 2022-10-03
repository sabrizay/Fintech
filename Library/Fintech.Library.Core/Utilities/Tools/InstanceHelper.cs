using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.Library.Core.Utilities.Tools
{
    public static class InstanceHelper
    {
        public static dynamic GetInstance(InstanceParameter instanceParameter)
        {
            var model = Assembly.Load(instanceParameter.ModelNamespace);
            var type = model.GetTypes().FirstOrDefault(t => t.FullName.Split('.').LastOrDefault().Equals(instanceParameter.ClassName));
 
         
           return type.Assembly.CreateInstance(type.FullName);
         

        }
    }

   public class InstanceParameter
    {
        public string ClassName { get; set; }
        public string ModelNamespace { get; set; }
    } 
}
