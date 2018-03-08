using System;
using System.Threading.Tasks;
using ADMIN.Models;
using Microsoft.EntityFrameworkCore;

namespace ADMIN.Data
{
    public class AuthRepository : IAuthRepository
    {

        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<UsuariosDASHBOARDMIAUTO> Login(string correo, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Correo_Electronico == correo);

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            //auth success!!

            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }

            return true;
        }

        public async Task<UsuariosDASHBOARDMIAUTO> Register(UsuariosDASHBOARDMIAUTO usuario, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            usuario.PasswordHash = passwordHash;
            usuario.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(usuario);

            await _context.SaveChangesAsync();

            return usuario;

        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }

        public async Task<bool> EmailExists(string correo)
        {
            if (await _context.Users.AnyAsync(x => x.Correo_Electronico == correo))
                return true;

            return false;
        }
    }
}