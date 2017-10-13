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
    /// Información de un pedido cabecera sin especificar más
    /// </summary>
    [Table("PedidosLineas")]
    public class PedidoLinea
    {

        [Key]
        public int PedidoLineaId { get; set; }

        [DisplayName("Volumen"), Display(Name = "Volumen")]
        public double? Volumen { get; set; }

        [Range(0, 1000)]
        [DisplayName("Unidades"), Display(Name = "Unidades")]
        public int? Unidades { get; set; }

        public int PedidoCabeceraId { get; set; }

        public int TipoProductoEnvasadoId { get; set; }

        [ForeignKey("PedidoCabeceraId")]
        public virtual PedidoCabecera PedidoCabecera { get; set; }

        [ForeignKey("TipoProductoEnvasadoId")]
        public virtual TipoProductoEnvasado TipoProductoEnvasado { get; set; }

        public virtual List<PedidoDetalle> PedidoDetalles { get; set; }
    }
}
