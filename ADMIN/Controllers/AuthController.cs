using System.Threading.Tasks;
using ADMIN.Data;
using ADMIN.Dtos;
using ADMIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace ADMIN.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserForRegisterDto UserForRegisterDto)
        {

            // Validar el request

            UserForRegisterDto.Email = UserForRegisterDto.Email.ToLower();

            if (await _repo.EmailExists(UserForRegisterDto.Email))
                ModelState.AddModelError("Email", "Email ya registrado");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userToCreate = new UsuariosDASHBOARDMIAUTO
            {
                Nombre_Usuario = UserForRegisterDto.Nombre,
                Correo_Electronico = UserForRegisterDto.Email
            };

            var createUser = await _repo.Register(userToCreate, UserForRegisterDto.Password);



            return Ok(new { usuarioCreado= createUser.Nombre_Usuario});
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserForLoginDto UserForLoginDto)
        {
            var userFromRepo = await _repo.Login(UserForLoginDto.Correo.ToLower(), UserForLoginDto.Contrasena);

            if (userFromRepo == null)
                return Unauthorized();


            //Aqui se genera el token

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                    new Claim(ClaimTypes.Name, userFromRepo.Nombre_Usuario)

                }),
                Expires = System.DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { token= tokenString, usuario = userFromRepo, id= userFromRepo.Id.ToString() });

        }


    }
}