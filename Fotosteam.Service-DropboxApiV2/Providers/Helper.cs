using System;
using System.Security.Cryptography;
using System.Text;

namespace Fotosteam.Service.Providers
{
    /// <summary>
    ///     Hilfsklasse für spezielle Funktionen, die von unterschiedlichen Objekten genutzt werden können
    /// </summary>
    public class Helper
    {
        /// <summary>
        ///     Liefert einen eindeutigen HashKey für einen String zurück
        /// </summary>
        /// <param name="input">Wert zu dem der Hash ermittel werden soll</param>
        /// <returns>Hash als Zeichenfolge</returns>
        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();

            var byteValue = Encoding.UTF8.GetBytes(input);

            var byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }
    }
}