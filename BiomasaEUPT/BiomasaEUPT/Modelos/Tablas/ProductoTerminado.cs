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
    [Table("ProductosTerminados")]
    public class ProductoTerminado
    {
        [Key]
        public int ProductoTerminadoId { get; set; }

        [DisplayName("Peso"), Display(Name = "Peso")]
        public double? Peso { get; set; }

        [DisplayName("Volumen"), Display(Name = "Volumen")]
        public double? Volumen { get; set; }

        [Range(0, 1000)]
        [DisplayName("Unidades"), Display(Name = "Unidades")]
        public int? Unidades { get; set; }

        [StringLength(60)]
        [DisplayName("Observaciones"), Display(Name = "Observaciones")]
        public string Observaciones { get; set; }

        [DisplayName("Fecha alta"), Display(Name = "Fecha alta")]
        public DateTime FechaAlta { get; set; }

        [DisplayName("Fecha baja"), Display(Name = "Fecha baja")]
        public DateTime? FechaBaja { get; set; }

        public int TipoId { get; set; }

        public int GrupoId { get; set; }

        [Required]
        [StringLength(10)]
        [DisplayName("Código"), Display(Name = "Código")]
        public string Codigo { get; set; }

        [ForeignKey("TipoId")]
        public virtual TipoProductoTerminado TipoProductoTerminado { get; set; }

        [ForeignKey("GrupoId")]
        public virtual GrupoProductoTerminado GrupoProductoTerminado { get; set; }

        public virtual List<HuecoProducto> HuecosProductos { get; set; }
    }
}
