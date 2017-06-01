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
    [Table("OrdenesElaboraciones")]
    public class OrdenElaboracion
    {
        [Key]
        public int OrdenElaboracionId { get; set; }

        public int ProductoTerminadoId { get; set; }

        public int EstadoElaboracionId { get; set; }

        public int HuecoElaboracionId { get; set; }

        [DisplayName("Fecha alta"), Display(Name = "Fecha alta")]
        public DateTime? FechaAlta { get; set; }

        [ForeignKey("ProductoTerminadoId")]
        public virtual ProductoTerminado ProductoTerminado { get; set; }

        [ForeignKey("EstadoElaboracionId")]
        public virtual EstadoElaboracion EstadoElaboracion { get; set; }

        [ForeignKey("HuecoElaboracionId")]
        public virtual HuecoElaboracion HuecoElaboracion { get; set; }
    }
}
