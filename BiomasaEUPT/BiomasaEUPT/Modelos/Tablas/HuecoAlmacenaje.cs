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
    [Table("HuecosAlmacenajes")]
    public class HuecoAlmacenaje
    {
        [Key]
        public int HuecoAlmacenajeId { get; set; }

        [DisplayName("Volumen totales"), Display(Name = "Volumen totales")]
        public double? VolumenTotales { get; set; }

        [Range(0, 1000)]
        [DisplayName("Unidades totales"), Display(Name = "Unidades totales")]
        public int? UnidadesTotales { get; set; }

        [DisplayName("Ocupado"), Display(Name = "Ocupado")]
        public bool Ocupado { get; set; }

        public int SitioId { get; set; }

        [ForeignKey("SitioId")]
        public virtual SitioAlmacenaje SitioAlmacenaje { get; set; }

        public virtual List<HuecoProducto> HuecosProductos { get; set; }
    }
}
