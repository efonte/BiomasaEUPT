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
    /// Pedido de productos envasados para su venta a un cliente
    /// </summary>
    [Table("Pedidos")]
    public class Pedido
    {
        [Key]
        public int PedidoId { get; set; }

        [DisplayName("Fecha pedido"), Display(Name = "Fecha pedido")]
        public DateTime FechaPedido { get; set; }

        [DisplayName("Fecha finalización"), Display(Name = "Fecha finalización")]
        public DateTime? FechaFinalizacion { get; set; }
    }
}
