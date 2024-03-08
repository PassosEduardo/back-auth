using Autenticacao_AutorizaçãoApi.Models;
using Autenticacao_AutorizaçãoApi.Repositories;
using Autenticacao_AutorizaçãoApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autenticacao_AutorizaçãoApi.Controllers
{
    [ApiController]
    [Route("v1/auth")]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] User model)
        {
            var user = UserRepository.Get(model.Username, model.Password);

            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            var token = TokenService.GenerateToken(user);

            user.Password = "";

            return new
            {
                user = user,
                token = token
            };
            
        }

        [HttpGet]
        [Route("any")]
        [AllowAnonymous]
        public string Anonymous() => "Anônimo";

        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public string Authenticated() => String.Format("Autenticado - {0}", User.Identity.Name);

        [HttpGet]
        [Route("employee")]
        [Authorize(Roles = "employee,manager")]
        public string Employee() => "Funcionário";

        [HttpGet]
        [Route("manager")]
        [Authorize(Roles = "manager")]
        public string Manager() => "Gerente";

    }
}
