using System.Threading.Tasks;
using ADMIN.Models;

namespace ADMIN.Data
{
    public interface IAuthRepository
    {
        Task<UsuariosDASHBOARDMIAUTO> Register(UsuariosDASHBOARDMIAUTO user, string password);

        Task<UsuariosDASHBOARDMIAUTO> Login(string username, string password);

        Task<bool> EmailExists(string correo);



    }
}