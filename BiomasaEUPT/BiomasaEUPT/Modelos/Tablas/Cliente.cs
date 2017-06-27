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
    /// Cliente al que se le vendes los productos ventas
    /// </summary>
    [Table("Clientes")]
    public class Cliente
    {
        [Key]
        public int ClienteId { get; set; }

        [Required]
        [RegularExpression(Constantes.REGEX_RAZON_SOCIAL)]
        [StringLength(Constantes.LONG_MAX_RAZON_SOCIAL, MinimumLength = Constantes.LONG_MIN_RAZON_SOCIAL)]
        [Index(IsUnique = true)]
        [DisplayName("Razón social"), Display(Name = "Razón social")]
        public string RazonSocial { get; set; }

        [Required]
        [RegularExpression(Constantes.REGEX_NIF)]
        [StringLength(Constantes.LONG_MAX_NIF, MinimumLength = Constantes.LONG_MIN_NIF)]
        [Index(IsUnique = true)]
        [DisplayName("NIF"), Display(Name = "NIF")]
        public string Nif { get; set; }

        [Required]
        [StringLength(Constantes.LONG_MAX_EMAIL, MinimumLength = Constantes.LONG_MIN_EMAIL)]
        [Index(IsUnique = true)]
        [EmailAddress]
        [DisplayName("Email"), Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(Constantes.LONG_MAX_OBSERVACIONES, MinimumLength = Constantes.LONG_MIN_OBSERVACIONES)]
        [DisplayName("Observaciones"), Display(Name = "Observaciones")]
        public string Observaciones { get; set; }

        [Required]
        [RegularExpression(Constantes.REGEX_CALLE)]
        [StringLength(Constantes.LONG_MAX_CALLE, MinimumLength = Constantes.LONG_MIN_CALLE)]
        [DisplayName("Calle"), Display(Name = "Calle")]
        public string Calle { get; set; }

        public int MunicipioId { get; set; }

        public int TipoId { get; set; }

        public int GrupoId { get; set; }

        [ForeignKey("MunicipioId")]
        public virtual Municipio Municipio { get; set; }

        [ForeignKey("TipoId")]
        public virtual TipoCliente TipoCliente { get; set; }

        [ForeignKey("GrupoId")]
        public virtual GrupoCliente GrupoCliente { get; set; }
    }
}
