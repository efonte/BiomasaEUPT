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
    [Table("HuecosProductos")]
    public class HuecoProducto
    {
        [Key]
        public int HuecoProductoId { get; set; }

        [DisplayName("Volumen"), Display(Name = "Volumen")]
        public float? Volumen { get; set; }

        [Range(0, 1000)]
        [DisplayName("Unidades"), Display(Name = "Unidades")]
        public int? Unidades { get; set; }

        [DisplayName("Volumen utilizado"), Display(Name = "Volumen utilizado")]
        public float? VolumenUtilizado { get; set; }

        [Range(0, 1000)]
        [DisplayName("Unidades utilizadas"), Display(Name = "Unidades utilizadas")]
        public int? UnidadesUtilizadas { get; set; }

        public int ProductoTerminadoId { get; set; }

        public int HuecoAlmacenajeId { get; set; }

        [ForeignKey("ProductoTerminadoId")]
        public virtual ProductoTerminado ProductoTerminado { get; set; }

        [ForeignKey("HuecoAlmacenajeId")]
        public virtual HuecoAlmacenaje HuecoAlmacenaje { get; set; }
    }
}
