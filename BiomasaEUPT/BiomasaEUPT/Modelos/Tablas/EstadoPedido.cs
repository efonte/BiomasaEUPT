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
    /// Topología de los distintos tipos de estados que puede tener una salida (Procesada y Enviada)
    /// </summary>
    [Table("EstadosSalidas")]
    public class EstadoPedido
    {
        [Key]
        public int EstadoSalidaId { get; set; }

        [Required]
        [StringLength(Constantes.LONG_MAX_NOMBRE_ESTADO_PEDIDO, MinimumLength = Constantes.LONG_MIN_NOMBRE_ESTADO_PEDIDO)]
        [DisplayName("Nombre"), Display(Name = "Nombre")]
        public string Nombre { get; set; }

    }
}
