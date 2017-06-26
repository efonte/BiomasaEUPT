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
    /// Materia prima que será usada para fabricar un producto terminado
    /// </summary>
    [Table("MateriasPrimas")]
    public class MateriaPrima
    {
        [Key]
        public int MateriaPrimaId { get; set; }

        [DisplayName("Volumen"), Display(Name = "Volumen")]
        public double? Volumen { get; set; }

        [Range(0, 1000)]
        [DisplayName("Unidades"), Display(Name = "Unidades")]
        public int? Unidades { get; set; }

        [StringLength(Constantes.LONG_MAX_OBSERVACIONES, MinimumLength = Constantes.LONG_MIN_OBSERVACIONES)]
        [DisplayName("Observaciones"), Display(Name = "Observaciones")]
        public string Observaciones { get; set; }

        [DisplayName("Fecha baja"), Display(Name = "Fecha baja")]
        public DateTime? FechaBaja { get; set; }

        public int TipoId { get; set; }

        public int RecepcionId { get; set; }

        public int ProcedenciaId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [StringLength(Constantes.LONG_MAX_CODIGO, MinimumLength = Constantes.LONG_MIN_CODIGO)]
        [DisplayName("Código"), Display(Name = "Código")]
        public string Codigo { get; set; }

        [ForeignKey("TipoId")]
        public virtual TipoMateriaPrima TipoMateriaPrima { get; set; }

        [ForeignKey("RecepcionId")]
        public virtual Recepcion Recepcion { get; set; }

        [ForeignKey("ProcedenciaId")]
        public virtual Procedencia Procedencia { get; set; }

        public virtual List<HistorialHuecoRecepcion> HistorialHuecosRecepciones { get; set; }
    }
}
