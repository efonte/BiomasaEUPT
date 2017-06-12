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
    /// Código ISO_3166-1 de cada país con su nombre
    /// </summary>
    [Table("Paises")]
    public class Pais
    {
        [Key]
        public int PaisId { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(2)]
        [Index(IsUnique = true)]
        [DisplayName("Código"), Display(Name = "Código")]
        public string Codigo { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        [Index(IsUnique = true)]
        [DisplayName("Nombre"), Display(Name = "Nombre")]
        public string Nombre { get; set; }

        public virtual List<Comunidad> Comunidades { get; set; }
    }
}
