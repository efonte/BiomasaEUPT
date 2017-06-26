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
    /// Picking es el equivalente a sitios recogidas
    /// </summary>
    [Table("Picking")]
    public class Picking
    {
        [Key]
        public int PickingId { get; set; }

        [Required]
        [StringLength(Constantes.LONG_MAX_NOMBRE, MinimumLength = Constantes.LONG_MIN_NOMBRE)]
        [Index(IsUnique = true)]
        [DisplayName("Nombre"), Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [DisplayName("Volumen total"), Display(Name = "Volumen total")]
        public double? VolumenTotal { get; set; }

        // Volumen restantes
        [DisplayName("Volumen restante"), Display(Name = "Volumen restante")]
        public double? VolumenRestante { get; set; }

        public virtual List<ProductoEnvasado> ProductosEnvasados { get; set; }
    }
}
