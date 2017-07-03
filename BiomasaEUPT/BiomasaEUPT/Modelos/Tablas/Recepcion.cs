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
    /// Cada una de las recepciones que llegan con materias primas
    /// </summary>
    [Table("Recepciones")]
    public class Recepcion
    {
        [Key]
        public int RecepcionId { get; set; }

        [Required]
        [DisplayName("Fecha recepción"), Display(Name = "Fecha recepción")]
        public DateTime FechaRecepcion { get; set; }

        [Required]
        [RegularExpression(Constantes.REGEX_NUMERO_ALBARAN)]
        [StringLength(Constantes.LONG_MAX_NUMERO_ALBARAN, MinimumLength = Constantes.LONG_MIN_NUMERO_ALBARAN)]
        [Index(IsUnique = true)]
        [DisplayName("Número de albarán"), Display(Name = "Número de albarán")]
        public string NumeroAlbaran { get; set; }

        public int ProveedorId { get; set; }

        public int EstadoId { get; set; }

        [ForeignKey("ProveedorId")]
        public virtual Proveedor Proveedor { get; set; }

        [ForeignKey("EstadoId")]
        public virtual EstadoRecepcion EstadoRecepcion { get; set; }

        public virtual List<MateriaPrima> MateriasPrimas { get; set; }
    }
}
