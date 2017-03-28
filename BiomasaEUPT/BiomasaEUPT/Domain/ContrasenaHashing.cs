using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BiomasaEUPT.Domain
{
    class ContrasenaHashing
    {
      /*  public static byte[] CalcularHash(byte[] bytesEntrada)
        {
            SHA256Managed algoritmo = new SHA256Managed();
            algoritmo.ComputeHash(bytesEntrada);
            return algoritmo.Hash;
        }

        public static bool SequenceEquals(byte[] originalByteArray, byte[] nuevoByteArray)
        {
            //If either byte array is null, throw an ArgumentNullException
            if (originalByteArray == null || nuevoByteArray == null)
                throw new ArgumentNullException(originalByteArray == null ? "originalByteArray" : "nuevoByteArray",
                                  "Los byte arrays proporcionados no pueden ser nulos.");

            //If byte arrays are different lengths, return false
            if (originalByteArray.Length != nuevoByteArray.Length)
            {
                return false;
            }
            //If any elements in corresponding positions are not equal
            //return false
            for (int i = 0; i < originalByteArray.Length; i++)
            {
                if (originalByteArray[i] != nuevoByteArray[i])
                    return false;
            }

            //If we've got this far, the byte arrays are equal.
            return true;
        }*/


        public static String obtenerHashSHA256(String cadena)
        {
            // http://stackoverflow.com/a/30618736
            using (SHA256 hash = SHA256Managed.Create())
            {
                return String.Concat(hash
                  .ComputeHash(Encoding.UTF8.GetBytes(cadena))
                  .Select(item => item.ToString("x2")));
            }
        }


        public static String SecureStringToString(SecureString cadenaSegura)
        {
            // http://stackoverflow.com/a/30618736
            IntPtr valorPtr = IntPtr.Zero;
            try
            {
                valorPtr = Marshal.SecureStringToGlobalAllocUnicode(cadenaSegura);
                return Marshal.PtrToStringUni(valorPtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valorPtr);
            }
        }

    }
}
