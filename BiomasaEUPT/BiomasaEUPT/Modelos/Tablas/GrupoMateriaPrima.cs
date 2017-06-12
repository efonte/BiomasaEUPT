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
    /// Topología de grupos de materias primas. Cada tipo de tipo de materia prima está asociado a un grupo.
    /// </summary>
    /// <example>
    /// Grupo Troncos, Grupo Pellets, Grupo Tablones
    /// </example>
    [Table("GruposMateriasPrimas")]
    public class GrupoMateriaPrima
    {
        [Key]
        public int GrupoMateriaPrimaId { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        [DisplayName("Nombre"), Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        [DisplayName("Descripción"), Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        public virtual List<TipoMateriaPrima> TiposMateriasPrimas { get; set; }
    }
}
