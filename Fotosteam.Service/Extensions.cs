using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Fotosteam.Service
{
    /// <summary>
    ///     Kapselt Erweiterungsmethoden
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        ///     Ändert der Wert einen Strings nur, wenn der neue Wert nicht leer oder null ist
        /// </summary>
        /// <param name="value">Der aktuelle Wert des Strings</param>
        /// <param name="newValue">Der neue Wert</param>
        /// <returns>Den neuen Wert, wenn er nicht null oder leer ist</returns>
        public static string NewOrDefault(this string value, string newValue)
        {
            return string.IsNullOrEmpty(newValue) ? value : value;
        }

        public static string MakeUrlSafe(this string value)
        {
            //IMPORTANT: Changes MUST transfered to tools.js > MakeUrlSafe

            if (string.IsNullOrEmpty(value)) { return ""; }
            
            StringBuilder stb = new StringBuilder(value);
            stb.Replace("ä", "ae");
            stb.Replace("ö", "oe");
            stb.Replace("ü", "ue");
            stb.Replace("ä", "ae");

            Regex rgx = new Regex("[^a-zA-Z0-9]+");

            var result = rgx.Replace(stb.ToString(), "-");

            return HttpUtility.UrlPathEncode(result);
        }
    }
}