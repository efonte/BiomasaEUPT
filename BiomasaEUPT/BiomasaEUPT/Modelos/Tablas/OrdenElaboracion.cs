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

        public int EstadoElaboracionId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DisplayName("Fecha elaboración"), Display(Name = "Fecha elaboración")]
        public DateTime? FechaElaboracion { get; set; }

        [ForeignKey("EstadoElaboracionId")]
        public virtual EstadoElaboracion EstadoElaboracion { get; set; }

    }
}
