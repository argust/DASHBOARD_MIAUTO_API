using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ADMIN.Dtos
{
    public class UserForRegisterDto
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "La contraseña debe tener un rango de entre 4 y 8 caracteres")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El correo es requerido")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
