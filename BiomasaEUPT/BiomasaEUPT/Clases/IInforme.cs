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
        string GenerarInformeRecepcion(Proveedor proveedor);

        string GenerarInformeMateriaPrima(Proveedor proveedor);

        string GenerarInformeProductoTerminado(List<Proveedor> proveedores);

        string GenerarInformeProductoEnvasado(List<Proveedor> proveedores);
    }
}
