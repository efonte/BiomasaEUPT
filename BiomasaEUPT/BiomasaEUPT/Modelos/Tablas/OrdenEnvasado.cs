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
    [Table("OrdenesEnvasados")]
    public class OrdenEnvasado
    {
        [Key]
        public int OrdenEnvasadoId { get; set; }

        public int EstadoEnvasadoId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DisplayName("Fecha envasado"), Display(Name = "Fecha envasado")]
        public DateTime? FechaEnvasado { get; set; }

        [StringLength(Constantes.LONG_MAX_DESCRIPCION, MinimumLength = Constantes.LONG_MIN_DESCRIPCION)]
        [DisplayName("Descripción"), Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [ForeignKey("EstadoEnvasadoId")]
        public virtual EstadoEnvasado EstadoEnvasado { get; set; }

        public virtual List<ProductoEnvasado> ProductoEnvasado { get; set; }

    }
}
