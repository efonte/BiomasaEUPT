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
    [Table("Municipios")]
    public class Municipio
    {
        [Key]
        public int MunicipioId { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        [DisplayName("Nombre"), Display(Name = "Nombre")]
        public string Nombre { get; set; }

        public double? Latitud { get; set; }

        public double? Longitud { get; set; }

        public int ProvinciaId { get; set; }

        [ForeignKey("ProvinciaId")]
        public virtual Provincia Provincia { get; set; }

    }
}
