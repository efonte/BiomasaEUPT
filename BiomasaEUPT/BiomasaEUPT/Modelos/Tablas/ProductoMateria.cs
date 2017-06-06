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
    [Table("ProductosMaterias")]
    public class ProductoMateria
    {

        [Key]
        public int ProductoMateriaId { get; set; }

        [DisplayName("Volumen"), Display(Name = "Volumen")]
        public double? Volumen { get; set; }

        [Range(0, 1000)]
        [DisplayName("Unidades"), Display(Name = "Unidades")]
        public int? Unidades { get; set; }

        public int HuecoMateriaId { get; set; }

        public int ProductoId { get; set; }

        [ForeignKey("HuecoMateriaId")]
        public virtual HuecoMateriaPrima HuecoMateriaPrima { get; set; }

        [ForeignKey("ProductoId")]
        public virtual ProductoTerminado ProductoTerminado { get; set; }

    }
}
