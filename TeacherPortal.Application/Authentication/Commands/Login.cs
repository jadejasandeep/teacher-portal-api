using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TeacherPortal.Application.Common.Exceptions;
using TeacherPortal.Application.Common.Interfaces;
using TeacherPortal.Application.Common.Models;

namespace TeacherPortal.Application.Authentication.Commands
{
    public class Login : IRequest<ApiResponse<string>>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginHandler : IRequestHandler<Login, ApiResponse<string>>
    {
        private readonly ITeacherDbContext _context;
        private readonly IConfiguration _configuration;

        public LoginHandler(ITeacherDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ApiResponse<string>> Handle(Login request, CancellationToken cancellationToken)
        {
            var user = await _context.Teachers.SingleOrDefaultAsync(u => u.Username == request.Username, cancellationToken);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException();
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.FirstName+" "+user.LastName),
                new Claim(ClaimTypes.PrimarySid, Convert.ToString(user.Id))
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials);
            return ApiResponse<string>.Success(new JwtSecurityTokenHandler().WriteToken(token), "Token generated successful");
        }
    }
}
