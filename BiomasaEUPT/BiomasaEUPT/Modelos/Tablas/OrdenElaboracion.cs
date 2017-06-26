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
    /// Orden de elaboración para crear un producto terminado usando materias primas
    /// </summary>
    [Table("OrdenesElaboraciones")]
    public class OrdenElaboracion
    {
        [Key]
        public int OrdenElaboracionId { get; set; }

        public int EstadoElaboracionId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DisplayName("Fecha elaboración"), Display(Name = "Fecha elaboración")]
        public DateTime? FechaElaboracion { get; set; }

        [StringLength(Constantes.LONG_MAX_DESCRIPCION, MinimumLength = Constantes.LONG_MIN_DESCRIPCION)]
        [DisplayName("Descripción"), Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [ForeignKey("EstadoElaboracionId")]
        public virtual EstadoElaboracion EstadoElaboracion { get; set; }

        public virtual List<ProductoTerminado> ProductoTerminado { get; set; }

    }
}
