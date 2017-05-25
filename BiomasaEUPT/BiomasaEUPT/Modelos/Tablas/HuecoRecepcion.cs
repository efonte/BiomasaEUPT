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
    [Table("HuecosRecepciones")]
    public class HuecoRecepcion
    {
        [Key]
        public int HuecoRecepcionId { get; set; }

        [DisplayName("Volumen"), Display(Name = "Volumen")]
        public float? Volumen { get; set; }

        [Range(0, 1000)]
        [DisplayName("Unidades"), Display(Name = "Unidades")]
        public int? Unidades { get; set; }

        public int SitioId { get; set; }

        [ForeignKey("SitioId")]
        public virtual SitioRecepcion SitioRecepcion { get; set; }
    }
}
