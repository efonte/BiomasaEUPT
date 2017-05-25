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
    [Table("Salidas")]
    public class Salida
    {
        [Key]
        public int SalidaId { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [DisplayName("Fecha entrega")]
        public DateTime FechaEntrega { get; set; }

        [Required]
        [StringLength(9)]
        public string Pedido { get; set; }

        public int EstadoId { get; set; }

        public int ClienteId { get; set; }

        [Required]
        [MinLength(3)]
        [StringLength(50)]
        public string Destino { get; set; }

        [ForeignKey("EstadoId")]
        public virtual EstadoSalida EstadoSalida { get; set; }

        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; }
    }
}
