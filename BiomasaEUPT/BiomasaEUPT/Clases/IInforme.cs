using BiomasaEUPT.Modelos.Tablas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiomasaEUPT.Clases
{
    public interface IInforme
    {
        string GenerarPDFRecepcion(Proveedor proveedor);

        string GenerarPDFMateriaPrima(Proveedor proveedor);

        string GenerarPDFProductoTerminado(List<Proveedor> proveedores);

        string GenerarPDFProductoEnvasado(List<Proveedor> proveedores);
    }
}
