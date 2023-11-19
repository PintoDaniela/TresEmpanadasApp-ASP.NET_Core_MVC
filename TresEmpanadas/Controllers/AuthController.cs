using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using TresEmpanadas.Helpers;
using TresEmpanadas.Interfaces;

namespace TresEmpanadas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly JwtTokenManager _jwtTokenManager;
        private readonly IAuthRepository _authRepository;

        public AuthController(IConfiguration config, JwtTokenManager jwtTokenManager, IAuthRepository authRepository)
        {
            _config = config;
            _jwtTokenManager = jwtTokenManager;
            _authRepository = authRepository;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Register([FromBody] UserDto newUser)
        {
            var existe = await _authRepository.ExisteUsuarioByNameAsync(newUser.UserName);
            if (existe)
            {
                return BadRequest($"Ya existe un usuario con el nombre {newUser.UserName}");
            }
            var usuarioCreado = await _authRepository.CreateUserAsync(newUser);

            if (usuarioCreado != null)
            {
                return CreatedAtAction("Register", usuarioCreado.Id, usuarioCreado);
            }
            return BadRequest();
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] UserDto loginUser)
        {
            var (credencialesOk, user) = await _authRepository.CheckUserCredentialsAsync(loginUser);
            if (!credencialesOk || user == null)
            {
                return Unauthorized("Credenciales inválidas");
            }
            var token = _jwtTokenManager.GenerateJwtToken(user, _config);
            return Ok(new { Token = token });
        }


        [HttpPost("validar-token")]
        public IActionResult ValidarToken([FromHeader] string token)
        {
            bool tokenValido = _jwtTokenManager.ValidateToken(token, _config);

            if (!tokenValido)
            {
                return Unauthorized("Token inválido. Inicie sesión para generar un nuevo token de acceso.");
            }

            return Ok("Token válido.");
        }
    }
}
