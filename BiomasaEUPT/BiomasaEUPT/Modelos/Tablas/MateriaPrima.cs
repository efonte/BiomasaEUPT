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
    [Table("MateriasPrimas")]
    public class MateriaPrima
    {
        [Key]
        public int MateriaPrimaId { get; set; }

        [DisplayName("Peso"), Display(Name = "Peso")]
        public double? Peso { get; set; }

        [DisplayName("Volumen"), Display(Name = "Volumen")]
        public double? Volumen { get; set; }

        [Range(0, 1000)]
        [DisplayName("Unidades"), Display(Name = "Unidades")]
        public int? Unidades { get; set; }

        [MaxLength(60)]
        [DisplayName("Observaciones"), Display(Name = "Observaciones")]
        public string Observaciones { get; set; }

        [DisplayName("Fecha baja"), Display(Name = "Fecha baja")]
        public DateTime? FechaBaja { get; set; }

        public int TipoId { get; set; }

        public int GrupoId { get; set; }

        public int RecepcionId { get; set; }

        public int ProcedenciaId { get; set; }

        [Required]
        [StringLength(10)]
        [DisplayName("Código"), Display(Name = "Código")]
        public string Codigo { get; set; }

        [ForeignKey("TipoId")]
        public virtual TipoMateriaPrima TipoMateriaPrima { get; set; }

        [ForeignKey("GrupoId")]
        public virtual GrupoMateriaPrima GrupoMateriaPrima { get; set; }

        [ForeignKey("RecepcionId")]
        public virtual Recepcion Recepcion { get; set; }

        [ForeignKey("ProcedenciaId")]
        public virtual Procedencia Procedencia { get; set; }

        public virtual List<HuecoMateriaPrima> HuecosMateriasPrimas { get; set; }
    }
}
