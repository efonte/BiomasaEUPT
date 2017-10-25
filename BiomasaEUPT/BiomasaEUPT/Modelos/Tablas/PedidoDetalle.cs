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
    /// Información detallada de un pedido realizado por un cliente
    /// </summary>
    [Table("PedidosDetalles")]
    public class PedidoDetalle
    {
        [Key]
        public int PedidoDetalleId { get; set; }

        [DisplayName("Volumen"), Display(Name = "Volumen")]
        public double? Volumen { get; set; }

        [Range(0, 1000)]
        [DisplayName("Unidades"), Display(Name = "Unidades Restante")]
        public int? Unidades { get; set; }

        public int PedidoLineaId { get; set; }

        public int ProductoEnvasadoId { get; set; }

        [ForeignKey("PedidoLineaId")]
        public virtual PedidoLinea PedidoLinea{ get; set; }

        [ForeignKey("ProductoEnvasadoId")]
        public virtual ProductoEnvasado ProductoEnvasado { get; set; }

    }
}
