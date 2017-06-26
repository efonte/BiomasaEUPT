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
    /// <summary>
    /// Producto obtenido al final del proceso de elaboración. Se utilizan materias primas
    /// </summary>
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

        [StringLength(Constantes.LONG_MAX_OBSERVACIONES, MinimumLength = Constantes.LONG_MIN_OBSERVACIONES)]
        [DisplayName("Observaciones"), Display(Name = "Observaciones")]
        public string Observaciones { get; set; }

        [DisplayName("Fecha baja"), Display(Name = "Fecha baja")]
        public DateTime? FechaBaja { get; set; }

        public int TipoId { get; set; }

        public int OrdenId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [StringLength(Constantes.LONG_MAX_CODIGO, MinimumLength = Constantes.LONG_MIN_CODIGO)]
        [DisplayName("Código"), Display(Name = "Código")]
        public string Codigo { get; set; }

        [ForeignKey("TipoId")]
        public virtual TipoProductoTerminado TipoProductoTerminado { get; set; }

        [ForeignKey("OrdenId")]
        public virtual OrdenElaboracion OrdenElaboracion { get; set; }

        public virtual List<HistorialHuecoAlmacenaje> HistorialHuecosAlmacenajes { get; set; }

        public virtual List<ProductoTerminadoComposicion> ProductosTerminadosComposiciones { get; set; }
    }
}
