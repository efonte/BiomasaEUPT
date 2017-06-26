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
    /// Lugar de procedencia de la materia prima
    /// </summary>
    [Table("Procedencias")]
    public class Procedencia
    {
        [Key]
        public int ProcedenciaId { get; set; }

        [Required]
        [StringLength(Constantes.LONG_MAX_NOMBRE, MinimumLength = Constantes.LONG_MIN_NOMBRE)]
        [Index(IsUnique = true)]
        [DisplayName("Nombre"), Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(Constantes.LONG_MAX_DESCRIPCION, MinimumLength = Constantes.LONG_MIN_DESCRIPCION)]
        [DisplayName("Descripción"), Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        public virtual List<MateriaPrima> MateriasPrimas { get; set; }
    }
}
