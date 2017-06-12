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
    /// Comunidades de un país
    /// </summary>
    [Table("Comunidades")]
    public class Comunidad
    {
        [Key]
        public int ComunidadId { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(5)]
        [Index(IsUnique = true)]
        [DisplayName("Código"), Display(Name = "Código")]
        public string Codigo { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        [DisplayName("Nombre"), Display(Name = "Nombre")]
        public string Nombre { get; set; }

        public int PaisId { get; set; }

        [ForeignKey("PaisId")]
        public virtual Pais Pais { get; set; }

        public virtual List<Provincia> Provincias { get; set; }

    }
}
