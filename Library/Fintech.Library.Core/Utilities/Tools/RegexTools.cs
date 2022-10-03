using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Fintech.Library.Core.Utilities.Tools
{
    public static class RegexTools
    {
        // Yazı içerisinde modalog.com dışında url verilmişse # yapar.
        public static string RemoveOutLink(string inputString)
        {
            MatchCollection matches;
            string HRefPattern = @"href\s*=\s*(?:[""'](?<1>[^""']*)[""']|(?<1>\S+))";

            try
            {
                matches = Regex.Matches(inputString, HRefPattern,
                    RegexOptions.IgnoreCase | RegexOptions.Compiled,
                    TimeSpan.FromSeconds(1));
                foreach (Match item in matches)
                {
                    if (!item.Groups[1].Value.StartsWith("https://www.modalog.com"))
                        inputString = inputString.Replace(item.Groups[1].Value, "#");
                }

            }
            catch (RegexMatchTimeoutException)
            {
                //string cc = "The matching operation timed out.";
                return inputString;
            }

            return inputString;
        }

        public static string RemoveHtmlTags(string inputString)
        {
            //Remove scripts
            inputString = Regex.Replace(inputString, "`<script.*?>`.*?`</script>`", "", RegexOptions.Singleline);

            //Remove CSS styles, if any found
            inputString = Regex.Replace(inputString, "`<style.*?>`(.| )*?`</style>`", "", RegexOptions.Singleline);

            //Remove all HTML tags, leaving on the text inside.
            // inputString = Regex.Replace(inputString, "`<(.| )*?>`", "", RegexOptions.Singleline);

            //Remove \r,\t,\n
            // inputString = inputString.Replace("\r", "").Replace("\n", "").Replace("\t", "");

            return inputString;
        }

        public static string RemoveSqlCommands(string inputString)
        {

            string outputString = inputString.Trim();
            //outputString = outputString.Replace("&gt;", "");
            //outputString = outputString.Replace("&lt;", "");
            //outputString = outputString.Replace(">", "");
            //outputString = outputString.Replace("<", "");
            //outputString = outputString.Replace("--", "");
            //outputString = outputString.Replace("'", "");
            outputString = outputString.Replace(";", "");
            outputString = outputString.Replace("char ", "");
            outputString = outputString.Replace("delete ", "");
            outputString = outputString.Replace("insert ", "");
            outputString = outputString.Replace("update ", "");
            outputString = outputString.Replace("select ", "");
            outputString = outputString.Replace("truncate ", "");
            outputString = outputString.Replace("union", "");
            outputString = outputString.Replace("script ", "");

            return outputString;


        }
    }
}
