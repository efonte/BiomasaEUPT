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
    /// Distintos tipos de materias primas que existen dentro de la base de datos
    /// </summary>
    [Table("TiposMateriasPrimas")]
    public class TipoMateriaPrima
    {
        [Key]
        public int TipoMateriaPrimaId { get; set; }

        [Required]
        [StringLength(Constantes.LONG_MAX_NOMBRE, MinimumLength = Constantes.LONG_MIN_NOMBRE)]
        [Index(IsUnique = true)]
        [DisplayName("Nombre"), Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(Constantes.LONG_MAX_DESCRIPCION, MinimumLength = Constantes.LONG_MIN_DESCRIPCION)]
        [DisplayName("Descripción"), Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [DisplayName("Medido en volumen"), Display(Name = "Medido en volumen")]
        public bool? MedidoEnVolumen { get; set; }

        [DisplayName("Medido en unidades"), Display(Name = "Medido en unidades")]
        public bool? MedidoEnUnidades { get; set; }

        public int GrupoId { get; set; }

        [ForeignKey("GrupoId")]
        public virtual GrupoMateriaPrima GrupoMateriaPrima { get; set; }

        public virtual List<MateriaPrima> MateriasPrimas { get; set; }
    }
}
