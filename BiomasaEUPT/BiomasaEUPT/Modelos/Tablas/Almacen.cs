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
    [Table("Almacenes")]
    public class Almacen
    {
        [Key]
        public int AlmacenId { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        [DisplayName("Nombre"), Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [DisplayName("Humedad"), Display(Name = "Humedad")]
        public float? Humedad { get; set; }

        public int TipoMateriaPrimaId { get; set; }

        [ForeignKey("TipoMateriaPrimaId")]
        public virtual TipoMateriaPrima TipoMateriaPrima { get; set; }
    }
}
