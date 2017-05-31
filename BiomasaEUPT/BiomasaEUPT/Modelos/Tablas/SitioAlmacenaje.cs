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
    [Table("SitiosAlmacenajes")]
    public class SitioAlmacenaje
    {
        [Key]
        public int SitioAlmacenajeId { get; set; }

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

        //public int? TipoProductoTerminadoId { get; set; }

        //[ForeignKey("TipoProductoTerminadoId")]
        //public virtual TipoProductoTerminado TipoProductoTerminado { get; set; }

        public virtual List<HuecoAlmacenaje> HuecosAlmacenajes { get; set; }
    }
}
