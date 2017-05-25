using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedCodigosPostales
{
    class Program
    {
        static void Main(string[] args)
        {
            var seedCP = new SeedCP();
            seedCP.Generar();
        }
    }
}
