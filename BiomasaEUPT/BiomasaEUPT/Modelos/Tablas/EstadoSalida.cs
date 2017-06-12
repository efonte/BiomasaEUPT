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
    public class EstadoSalida
    {
        [Key]
        public int EstadoSalidaId { get; set; }

        [Required]
        [MinLength(3)]
        [StringLength(10)]
        [DisplayName("Nombre"), Display(Name = "Nombre")]
        public string Nombre { get; set; }

        public virtual List<Salida> Salidas { get; set; }
    }
}
