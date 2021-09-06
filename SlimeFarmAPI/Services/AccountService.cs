using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SlimeFarmAPI.DTOs;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace SlimeFarmAPI.Services {
    public class AccountService {
        private readonly DatabaseService database;
        private readonly IConfiguration configuration;

        public AccountService(IConfiguration configuration, DatabaseService database) {
            this.configuration = configuration;
            this.database = database;
        }

        public async Task<string> LoginAsync(LoginDTO loginDTO) {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));
            uint tokenLifetime = configuration.GetValue<uint>("JwtSettings:TokenLifetime");

            AccountDTO accountDTO = await database.GetAccountAsync(email: loginDTO.Email);
            byte[] salt = Convert.FromBase64String(accountDTO.Salt);

            string password = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: loginDTO.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 100000,
                numBytesRequested: 512 / 8));

            if (password != accountDTO.Password)
                return null;

            var claims = new List<Claim>() {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, accountDTO.Id.ToString())
            };

            var handler = new JwtSecurityTokenHandler();

            var token = handler.CreateEncodedJwt(
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                subject: new ClaimsIdentity(claims),
                notBefore: DateTime.UtcNow,
                issuedAt: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(tokenLifetime),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha512)
            );

            return token;
        }

        public async Task<string> RegisterAsync(AccountDTO accountDTO) {
            if (await database.GetAccountAsync(email: accountDTO.Email) != null)
                return "E-mail already registered!";
            else if (await database.GetAccountAsync(nickname: accountDTO.Nickname) != null)
                return "Nickname is taken!";

            byte[] salt = new byte[128 / 8];
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetNonZeroBytes(salt);

            accountDTO.Salt = Convert.ToBase64String(salt);
            accountDTO.Password = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: accountDTO.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 100000,
                numBytesRequested: 512 / 8));

            await database.InsertAccountAsync(accountDTO);
            //await database.InsertDefaultsAsync(accountDTO.Id);
            return null;
        }

        public string Refresh(string token) {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));
            uint tokenLifetime = configuration.GetValue<uint>("JwtSettings:TokenLifetime");
            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwt = handler.ReadJwtToken(token);

            token = handler.CreateEncodedJwt(
                issuer: jwt.Issuer,
                audience: jwt.Audiences.First(),
                subject: new ClaimsIdentity(jwt.Claims),
                notBefore: DateTime.UtcNow,
                issuedAt: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(tokenLifetime),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha512)
            );

            return token;
        }

        public async Task<bool> ChangePasswordAsync() {
            throw new NotImplementedException();
        }
    }
}
