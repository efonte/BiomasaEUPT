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
    /// Usuarios que están registrados en el sistema para utilizar la aplicación
    /// </summary>
    [Table("Usuarios")]
    public class Usuario
    {

        public Usuario()
        {
            Baneado = false;
        }

        [Key]
        public int UsuarioId { get; set; }

        [Required]
        [StringLength(Constantes.LONG_MAX_NOMBRE_USUARIO, MinimumLength = Constantes.LONG_MIN_NOMBRE_USUARIO)]
        [Index(IsUnique = true)]
        [DisplayName("Nombre"), Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(Constantes.LONG_MAX_HASH_CONTRASENA, MinimumLength = Constantes.LONG_MIN_HASH_CONTRASENA)]
        [DisplayName("Contraseña"), Display(Name = "Contraseña")]
        public string Contrasena { get; set; }

        [Required]
        [StringLength(Constantes.LONG_MAX_EMAIL, MinimumLength = Constantes.LONG_MIN_EMAIL)]
        [Index(IsUnique = true)]
        [EmailAddress]
        [DisplayName("Email"), Display(Name = "Email")]
        public string Email { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DisplayName("Fecha alta"), Display(Name = "Fecha alta")]
        public DateTime? FechaAlta { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DisplayName("Fecha baja"), Display(Name = "Fecha baja")]
        public DateTime? FechaBaja { get; set; }

        [DisplayName("Fecha contraseña"), Display(Name = "Fecha contraseña")]
        public DateTime? FechaContrasena { get; set; }

        [DisplayName("Baneado"), Display(Name = "Baneado")]
        public bool? Baneado { get; set; }

        public int TipoId { get; set; }

        [ForeignKey("TipoId")]
        public virtual TipoUsuario TipoUsuario { get; set; }
    }
}
