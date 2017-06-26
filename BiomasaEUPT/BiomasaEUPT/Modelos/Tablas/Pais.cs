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
        [StringLength(Constantes.LONG_MAX_CODIGO_PAIS, MinimumLength = Constantes.LONG_MIN_CODIGO_PAIS)]
        [Index(IsUnique = true)]
        [DisplayName("Código"), Display(Name = "Código")]
        public string Codigo { get; set; }

        [Required]
        [StringLength(Constantes.LONG_MAX_NOMBRE_PAIS, MinimumLength = Constantes.LONG_MIN_NOMBRE_PAIS)]
        [Index(IsUnique = true)]
        [DisplayName("Nombre"), Display(Name = "Nombre")]
        public string Nombre { get; set; }

        public virtual List<Comunidad> Comunidades { get; set; }
    }
}
