//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BiomasaEUPT
{
    using System;
    using System.Collections.Generic;
    
    public partial class clientes
    {
        public int id_cliente { get; set; }
        public string razon_social { get; set; }
        public string nif { get; set; }
        public string email { get; set; }
        public string observaciones { get; set; }
        public string calle { get; set; }
        public int direccion_id { get; set; }
        public int grupo_id { get; set; }
        public int tipo_id { get; set; }
    
        public virtual grupos_clientes grupos_clientes { get; set; }
        public virtual tipos_clientes tipos_clientes { get; set; }
        public virtual direcciones direcciones { get; set; }
    }
}
