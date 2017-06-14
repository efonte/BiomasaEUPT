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
        [MinLength(3)]
        [MaxLength(20)]
        [DisplayName("Nombre"), Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        [DisplayName("Descripción"), Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [DisplayName("Volumen"), Display(Name = "Volumen")]
        public float? Volumen { get; set; }

        [StringLength(60)]
        [DisplayName("Observaciones"), Display(Name = "Observaciones")]
        public string Observaciones { get; set; }

        [DisplayName("Fecha baja"), Display(Name = "Fecha baja")]
        public DateTime? FechaBaja { get; set; }

        [Required]
        [StringLength(10)]
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
