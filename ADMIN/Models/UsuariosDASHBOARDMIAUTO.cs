using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ADMIN.Models
{
    [Table("UsuariosDASHBOARDMIAUTO")]
    public class UsuariosDASHBOARDMIAUTO
    {
        public int Id { get; set; }

        public string Nombre_Usuario { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Correo_Electronico { get; set; }
    }
}
