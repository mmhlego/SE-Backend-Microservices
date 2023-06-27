using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtAuthenticationManager.Models;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthenticationManager
{
	public class JwtTokenHandler
	{
		public const string JWT_SECURITY_KEY = "2D93910C652F3FC5466B679069F7E929";
		private const int JWT_VALIDITY_MINS = 60 * 24 * 4;

		public AuthenticationResponse GenerateJwtToken(string userName, string userId, string userRole)
		{
			var expireTimeStamp = DateTime.Now.AddMinutes(JWT_VALIDITY_MINS);
			var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
			var claimsIdentity = new ClaimsIdentity(new List<Claim> {
				new Claim(ClaimTypes.NameIdentifier, userId),
				new Claim(ClaimTypes.Role, userRole)
			});

			var signingCredentials = new SigningCredentials(
				new SymmetricSecurityKey(tokenKey),
				SecurityAlgorithms.HmacSha256Signature
			);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = claimsIdentity,
				Expires = expireTimeStamp,
				SigningCredentials = signingCredentials,
			};

			var jwtTokenHandler = new JwtSecurityTokenHandler();
			var securityToken = jwtTokenHandler.CreateToken(tokenDescriptor);

			var token = jwtTokenHandler.WriteToken(securityToken);

			return new AuthenticationResponse
			{
				JwtToken = token,
				UserName = userName,
				Role = userRole,
				ExpiresIn = (int)expireTimeStamp.Subtract(DateTime.Now).TotalSeconds,
			};
		}
	}
}
