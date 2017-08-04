using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiomasaEUPT.Modelos.Tablas
{
    public enum Tab
    {
        Permisos,
        Usuarios,
        Clientes,
        Proveedores,
        Recepciones,
        Elaboraciones,
        Ventas,
        Trazabilidad
    }


    /// <summary>
    /// Permisos que tienen los tipos de usuarios que hay en la aplicación.
    /// </summary>
    [Table("Permisos")]
    public class Permiso
    {
        [Key]
        public int PermisoId { get; set; }

        public Tab Tab { get; set; }

        public int TipoId { get; set; }

        [ForeignKey("TipoId")]
        public virtual TipoUsuario TipoUsuario { get; set; }
    }
}
