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
    /// Producto final listo para venta
    /// </summary>
    [Table("ProductosEnvasados")]
    public class ProductoEnvasado
    {
        [Key]
        public int ProductoEnvasadoId { get; set; }

        [Required]
        [StringLength(Constantes.LONG_MAX_NOMBRE, MinimumLength = Constantes.LONG_MIN_NOMBRE)]
        [DisplayName("Nombre"), Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [DisplayName("Volumen"), Display(Name = "Volumen")]
        public float? Volumen { get; set; }

        [StringLength(Constantes.LONG_MAX_OBSERVACIONES, MinimumLength = Constantes.LONG_MIN_OBSERVACIONES)]
        [DisplayName("Observaciones"), Display(Name = "Observaciones")]
        public string Observaciones { get; set; }

        [DisplayName("Fecha baja"), Display(Name = "Fecha baja")]
        public DateTime? FechaBaja { get; set; }

        [Required]
        [StringLength(Constantes.LONG_MAX_CODIGO, MinimumLength = Constantes.LONG_MIN_CODIGO)]
        [DisplayName("Código"), Display(Name = "Código")]
        public string Codigo { get; set; }

        public int TipoProductoTerminadoId { get; set; }

        public int PickingId { get; set; }

        [ForeignKey("TipoProductoTerminadoId")]
        public virtual TipoProductoTerminado TipoProductoTerminado { get; set; }

        [ForeignKey("PickingId")]
        public virtual Picking Picking { get; set; }

        public virtual List<ProductoEnvasadoComposicion> ProductosEnvasadosComposiciones { get; set; }

    }
}
