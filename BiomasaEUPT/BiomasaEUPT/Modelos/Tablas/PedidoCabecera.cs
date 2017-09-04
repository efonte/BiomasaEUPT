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
    /// Pedido que realiza un cliente
    /// </summary>
    [Table("PedidosCabeceras")]
    public class PedidoCabecera
    {
        [Key]
        public int PedidoCabeceraId { get; set; }

        [DisplayName("Fecha pedido"), Display(Name = "Fecha pedido")]
        public DateTime FechaPedido { get; set; }

        [DisplayName("Fecha finalización"), Display(Name = "Fecha finalización")]
        public DateTime? FechaFinalizacion { get; set; }

        public int EstadoId { get; set; }

        public int ClienteId { get; set; }

        [ForeignKey("EstadoId")]
        public virtual EstadoPedido EstadoPedido { get; set; }

        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; }

        public virtual List<PedidoDetalle> PedidoDetalles { get; set; }
    }
}
