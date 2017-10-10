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
    }
}
