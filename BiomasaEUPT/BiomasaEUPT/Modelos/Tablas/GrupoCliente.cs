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
    /// Topología de grupos de clientes. Cada tipo de cliente está asociado a un grupo.
    /// </summary>
    /// <example>
    /// Grupos varios (compran de todo), Grupo Pellets
    /// </example>
    [Table("GruposClientes")]
    public class GrupoCliente
    {
        [Key]
        public int GrupoClienteId { get; set; }

        [Required]
        [StringLength(Constantes.LONG_MAX_NOMBRE_GRUPO, MinimumLength = Constantes.LONG_MIN_NOMBRE_GRUPO)]
        [Index(IsUnique = true)]
        [DisplayName("Nombre"), Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(Constantes.LONG_MAX_DESCRIPCION_GRUPO, MinimumLength = Constantes.LONG_MIN_DESCRIPCION_GRUPO)]
        [DisplayName("Descripción"), Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        public virtual List<Cliente> Clientes { get; set; }
    }
}
