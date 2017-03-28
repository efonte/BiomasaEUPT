using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BiomasaEUPT.Domain
{
    class SecureStringManipulation
    {
        public static byte[] ConvertirSecureStringAByteArray(SecureString value)
        {
            //Byte array to hold the return value
            byte[] returnVal = new byte[value.Length];

            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(value);
                for (int i = 0; i < value.Length; i++)
                {
                    short unicodeChar = System.Runtime.InteropServices.Marshal.ReadInt16(valuePtr, i * 2);
                    returnVal[i] = Convert.ToByte(unicodeChar);
                }

                return returnVal;
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
    }
}
