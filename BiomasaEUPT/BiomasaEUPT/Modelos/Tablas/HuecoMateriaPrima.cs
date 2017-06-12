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
    /// Tabla que relaciona las materias primas que ha habido o hay en cada uno de los huecos recepción
    /// </summary>
    [Table("HuecosMateriasPrimas")]
    public class HuecoMateriaPrima
    {
        [Key]
        public int HuecoMateriaPrimaId { get; set; }

        [DisplayName("Volumen"), Display(Name = "Volumen")]
        public double? Volumen { get; set; }

        [Range(0, 1000)]
        [DisplayName("Unidades"), Display(Name = "Unidades")]
        public int? Unidades { get; set; }

        // Unidades y Volumen restantes son para realizar las elaboraciones
        [DisplayName("Volumen restantes"), Display(Name = "Volumen restantes")]
        public double? VolumenRestantes { get; set; }

        [Range(0, 1000)]
        [DisplayName("Unidades restantes"), Display(Name = "Unidades restantes")]
        public int? UnidadesRestantes { get; set; }

        public int MateriaPrimaId { get; set; }

        public int HuecoRecepcionId { get; set; }

        [ForeignKey("MateriaPrimaId")]
        public virtual MateriaPrima MateriaPrima { get; set; }

        [ForeignKey("HuecoRecepcionId")]
        public virtual HuecoRecepcion HuecoRecepcion { get; set; }

    }
}
