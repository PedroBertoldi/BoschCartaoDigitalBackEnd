using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BoschCartaoDigitalBackEnd.Database.Context;
using BoschCartaoDigitalBackEnd.Models.v1.Request.Autenticacao;
using BoschCartaoDigitalBackEnd.Models.v1.Responses.Autenticacao;
using BoschCartaoDigitalBackEnd.Models.v1.Responses.Commom;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BoschCartaoDigitalBackEnd.Controllers.v1.Autenticacao
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ProjetoBoschContext _db;

        public AutenticacaoController(ProjetoBoschContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }
        /// <summary>
        /// End-point para autenticação dos usuários.
        /// </summary>
        /// <param name="model">Dados necessários para a autenticação.</param>
        [HttpPost("[action]")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest model)
        {
            var user = await _db.Colaborador.Include(c => c.Permissao).ThenInclude(p => p.TipoPermissao).AsSplitQuery()
                .Where(c => c.Cpf == model.Cpf && c.DataNascimento.Value.Date == model.DataNascimento.Value.Date && c.UnidadeOrganizacionalId != null).FirstOrDefaultAsync();

            if (user != null)
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.NomeCompleto),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("id", user.Id.ToString()),
                };

                foreach (var permissao in user.Permissao)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, permissao.TipoPermissao.Descricao));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(Convert.ToInt32(_configuration["JWT:DurationMin"])),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new TokenResponse
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                    NomeCompleto = user.NomeCompleto,
                    id = user.Id,
                    Cargos = user.Permissao.Select(p => p.TipoPermissao.Descricao).ToList(),
                });
            }
            return Unauthorized(new ErrorResponse("Login ou senha invalido"));
        }
    }
}