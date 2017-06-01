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
    [Table("HuecosElaboraciones")]
    public class HuecoElaboracion
    {
        [Key]
        public int HuecoElaboracionId { get; set; }

        [DisplayName("Volumen utilizado"), Display(Name = "Volumen restantes")]
        public double? VolumenUtilizado { get; set; }

        [Range(0, 1000)]
        [DisplayName("Unidades utilizadas"), Display(Name = "Unidades utilizadas")]
        public int? UnidadesUtilizadas { get; set; }

        public int HuecoMateriaPrimaId { get; set; }

        [ForeignKey("HuecoMateriaPrimaId")]
        public virtual HuecoMateriaPrima HuecoMateriaPrima { get; set; }

        public virtual List<OrdenElaboracion> OrdenesElaboraciones { get; set; }
    }
}
