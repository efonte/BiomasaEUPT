﻿using System;
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

        [Required]
        [MinLength(3)]
        [MaxLength(3)]
        [StringLength(3, MinimumLength = 3)]
        [Index(IsUnique = true)]
        [DisplayName("Nombre"), Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [DisplayName("Volumen total"), Display(Name = "Volumen total")]
        public double VolumenTotal { get; set; }

        [Range(0, 1000)]
        [DisplayName("Unidades totales"), Display(Name = "Unidades totales")]
        public int UnidadesTotales { get; set; }

        public int SitioId { get; set; }

        [ForeignKey("SitioId")]
        public virtual SitioRecepcion SitioRecepcion { get; set; }

        public virtual List<HuecoMateriaPrima> HuecosMateriasPrimas { get; set; }
    }
}
