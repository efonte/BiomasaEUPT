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

        [DisplayName("Volumen"), Display(Name = "Volumen")]
        public double? Volumen { get; set; }

        [Range(0, 1000)]
        [DisplayName("Unidades"), Display(Name = "Unidades")]
        public int? Unidades { get; set; }

        [StringLength(60)]
        [DisplayName("Observaciones"), Display(Name = "Observaciones")]
        public string Observaciones { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DisplayName("Fecha elaboración"), Display(Name = "Fecha elaboración")]
        public DateTime? FechaElaboracion { get; set; }

        [DisplayName("Fecha baja"), Display(Name = "Fecha baja")]
        public DateTime? FechaBaja { get; set; }

        public int TipoId { get; set; }

        public int OrdenId { get; set; }

        [Required]
        [StringLength(10)]
        [DisplayName("Código"), Display(Name = "Código")]
        public string Codigo { get; set; }

        [ForeignKey("TipoId")]
        public virtual TipoProductoTerminado TipoProductoTerminado { get; set; }

        [ForeignKey("OrdenId")]
        public virtual OrdenElaboracion OrdenElaboracion { get; set; }

        public virtual List<HuecoProducto> HuecosProductos { get; set; }
    }
}
