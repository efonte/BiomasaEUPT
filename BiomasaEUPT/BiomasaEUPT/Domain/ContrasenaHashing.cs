using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BiomasaEUPT.Domain
{
    public class ContrasenaHashing
    {

        public static String ObtenerHashSHA256(String cadena)
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


        public static Boolean SecureStringEqual(SecureString secureString1, SecureString secureString2)
        {
            // https://stackoverflow.com/a/4502736
            if (secureString1 == null)
            {
                throw new ArgumentNullException("s1");
            }
            if (secureString2 == null)
            {
                throw new ArgumentNullException("s2");
            }

            if (secureString1.Length != secureString2.Length)
            {
                return false;
            }

            IntPtr ss_bstr1_ptr = IntPtr.Zero;
            IntPtr ss_bstr2_ptr = IntPtr.Zero;

            try
            {
                ss_bstr1_ptr = Marshal.SecureStringToBSTR(secureString1);
                ss_bstr2_ptr = Marshal.SecureStringToBSTR(secureString2);

                String str1 = Marshal.PtrToStringBSTR(ss_bstr1_ptr);
                String str2 = Marshal.PtrToStringBSTR(ss_bstr2_ptr);

                return str1.Equals(str2);
            }
            finally
            {
                if (ss_bstr1_ptr != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(ss_bstr1_ptr);
                }

                if (ss_bstr2_ptr != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(ss_bstr2_ptr);
                }
            }
        }

        public static bool IsEqualTo(SecureString ss1, SecureString ss2)
        {
            // https://stackoverflow.com/a/23183092
            IntPtr bstr1 = IntPtr.Zero;
            IntPtr bstr2 = IntPtr.Zero;
            try
            {
                bstr1 = Marshal.SecureStringToBSTR(ss1);
                bstr2 = Marshal.SecureStringToBSTR(ss2);
                int length1 = Marshal.ReadInt32(bstr1, -4);
                int length2 = Marshal.ReadInt32(bstr2, -4);
                if (length1 == length2)
                {
                    for (int x = 0; x < length1; ++x)
                    {
                        byte b1 = Marshal.ReadByte(bstr1, x);
                        byte b2 = Marshal.ReadByte(bstr2, x);
                        if (b1 != b2) return false;
                    }
                }
                else return false;
                return true;
            }
            finally
            {
                if (bstr2 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr2);
                if (bstr1 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr1);
            }
        }

    }
}
