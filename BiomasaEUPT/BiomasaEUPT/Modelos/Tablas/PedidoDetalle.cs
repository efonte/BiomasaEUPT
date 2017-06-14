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
    /// Información detallada de un pedido
    /// </summary>
    [Table("PedidosDetalles")]
    public class PedidoDetalle
    {
        [Key]
        public int PedidoDetalleId { get; set; }

        public int PedidoCabeceraId { get; set; }

        public int ProductoEnvasadoId { get; set; }

        [ForeignKey("PedidoCabeceraId")]
        public virtual PedidoCabecera PedidoCabecera { get; set; }

        [ForeignKey("ProductoEnvasadoId")]
        public virtual ProductoEnvasado ProductoEnvasado { get; set; }
    }
}
