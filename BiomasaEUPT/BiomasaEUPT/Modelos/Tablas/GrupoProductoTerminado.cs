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
    [Table("GruposProductosTerminados")]
    public class GrupoProductoTerminado
    {
        [Key]
        public int GrupoProductoTerminadoId { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        [Index(IsUnique = true)]
        [DisplayName("Nombre"), Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        [DisplayName("Descripción"), Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        public virtual List<ProductoTerminado> ProductosTerminados { get; set; }

        public virtual List<TipoProductoTerminado> TiposProductosTerminados { get; set; }
    }
}
