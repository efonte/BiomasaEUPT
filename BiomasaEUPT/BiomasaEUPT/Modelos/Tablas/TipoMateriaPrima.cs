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
    [Table("TiposMateriasPrimas")]
    public class TipoMateriaPrima
    {
        [Key]
        public int TipoMateriaPrimaId { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        [Index(IsUnique = true)]
        [DisplayName("Nombre"), Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        [DisplayName("Descripción"), Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [DisplayName("Medido en peso"), Display(Name = "Medido en peso")]
        public bool? MedidoEnPeso { get; set; }

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
