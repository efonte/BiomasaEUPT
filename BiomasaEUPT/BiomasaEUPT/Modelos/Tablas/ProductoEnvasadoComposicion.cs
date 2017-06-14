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
    /// Tabla que relaciona los productos terminados que se necesitan para componer un producto envasado
    /// </summary>
    [Table("ProductosEnvasadosComposiciones")]
    public class ProductoEnvasadoComposicion
    {
        [Key]
        public int ProductoEnvasadoComposicionId { get; set; }

        [DisplayName("Volumen"), Display(Name = "Volumen")]
        public double? Volumen { get; set; }

        [Range(0, 1000)]
        [DisplayName("Unidades"), Display(Name = "Unidades")]
        public int? Unidades { get; set; }

        public int HistorialHuecoId { get; set; }

        public int ProductoId { get; set; }

        [ForeignKey("HistorialHuecoId")]
        public virtual HistorialHuecoAlmacenaje HistorialHuecoAlmacenaje { get; set; }

        [ForeignKey("ProductoId")]
        public virtual ProductoEnvasado ProductoEnvasado{ get; set; }
    }
}
