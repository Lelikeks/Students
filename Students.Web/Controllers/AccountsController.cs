using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Students.Domain.Models;
using Students.Domain.Services;
using Students.Web.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Students.Web.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AccountsController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly IOptions<JwtOptions> _options;

		public AccountsController(IUserService userService, IOptions<JwtOptions> options)
		{
			this._userService = userService;
			this._options = options;
		}

		[HttpPost]
		public async Task<IActionResult> Token(User request)
		{
			var user = await _userService.GetUser(request.Login, request.Password);
			if (user == null)
			{
				return Unauthorized();
			}

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, _options.Value.Subject),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
				new Claim("Id", user.Id.ToString())
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Key));
			var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(_options.Value.Issuer, _options.Value.Audience, claims,
				expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

			return Ok(new JwtSecurityTokenHandler().WriteToken(token));
		}
	}
}
