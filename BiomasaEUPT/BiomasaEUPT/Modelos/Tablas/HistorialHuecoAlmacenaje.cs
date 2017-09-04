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
    /// Tabla que relaciona los productos terminados que ha habido o hay en cada uno de los huecos almacenaje
    /// </summary>
    [Table("HistorialHuecosAlmacenajes")]
    public class HistorialHuecoAlmacenaje
    {
        [Key]
        public int HistorialHuecoAlmacenajeId { get; set; }

        [DisplayName("Volumen"), Display(Name = "Volumen")]
        public double? Volumen { get; set; }

        [Range(0, 1000)]
        [DisplayName("Unidades"), Display(Name = "Unidades")]
        public int? Unidades { get; set; }

        // Unidades y Volumen restantes son para realizar los envasados
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DisplayName("Volumen restante"), Display(Name = "Volumen restante")]
        public double? VolumenRestante { get; set; }

        [Range(0, 1000)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DisplayName("Unidades restantes"), Display(Name = "Unidades restantes")]
        public int? UnidadesRestantes { get; set; }

        public int ProductoTerminadoId { get; set; }

        public int HuecoAlmacenajeId { get; set; }

        [ForeignKey("ProductoTerminadoId")]
        public virtual ProductoTerminado ProductoTerminado { get; set; }

        [ForeignKey("HuecoAlmacenajeId")]
        public virtual HuecoAlmacenaje HuecoAlmacenaje { get; set; }


    }
}
