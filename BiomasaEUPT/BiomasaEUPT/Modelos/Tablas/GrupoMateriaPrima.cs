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
