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
    /// Provincias de un país
    /// </summary>
    [Table("Provincias")]
    public class Provincia
    {
        [Key]
        public int ProvinciaId { get; set; }

        [Required]
        [StringLength(Constantes.LONG_MAX_CODIGO_PROVINCIA, MinimumLength = Constantes.LONG_MIN_CODIGO_PROVINCIA)]
        [Index(IsUnique = true)]
        [DisplayName("Código"), Display(Name = "Código")]
        public string Codigo { get; set; }

        [Required]
        [StringLength(Constantes.LONG_MAX_NOMBRE_PROVINCIA, MinimumLength = Constantes.LONG_MIN_NOMBRE_PROVINCIA)]
        [DisplayName("Nombre"), Display(Name = "Nombre")]
        public string Nombre { get; set; }

        public int ComunidadId { get; set; }

        [ForeignKey("ComunidadId")]
        public virtual Comunidad Comunidad { get; set; }

        public virtual List<Municipio> Municipios { get; set; }
    }
}
