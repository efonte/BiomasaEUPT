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
    [Table("Recepciones")]
    public class Recepcion
    {
        [Key]
        public int RecepcionId { get; set; }

        [Required]
        [DisplayName("Fecha recepción"), Display(Name = "Fecha recepción")]
        public DateTime FechaRecepcion { get; set; }

        [Required]
        [StringLength(10)]
        [Index(IsUnique = true)]
        [DisplayName("Número de albarán"), Display(Name = "Número de albarán")]
        public string NumeroAlbaran { get; set; }

        public int ProveedorId { get; set; }

        public int EstadoId { get; set; }

        [ForeignKey("ProveedorId")]
        public virtual Proveedor Proveedor { get; set; }

        [ForeignKey("EstadoId")]
        public virtual EstadoRecepcion EstadoRecepcion { get; set; }
    }
}
