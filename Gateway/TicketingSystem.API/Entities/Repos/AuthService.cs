﻿namespace Gateway.API.Entities.Repos
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
        Task<AuthModel> LoginAsync(LoginModel model);
        Task<AuthModel> RequestTokenAsync(TokenRequestModel model);
        Task<AuthModel> GetTokenAsync(ApplicationUser user);
        Task<AuthModel> RefreshTokenAsync(string refreshToken);
        Task<AuthModel> RevokeAndGenerate(string refreshToken);
        Task<bool> RevokeTokenAsync(string token);
        string GetRole(string roleName);
    }

    public class AuthService : IAuthService
    {
        #region Services
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWT _jwt;
        #endregion

        #region Constructors
        public AuthService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
        }
        #endregion

        #region Interface Implementation
        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthModel { Message = "Email is already registered.", IsAuthenticated = false };

            if (await _userManager.FindByNameAsync(model.UserName) is not null)
                return new AuthModel { Message = "Username is already registered.", IsAuthenticated = false };

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };
            // Adding new user
            var result = await _userManager.CreateAsync(user, model.Password);
            // Checking if the result is failure
            if (!result.Succeeded)
            {
                string errors = string.Empty;
                foreach (var error in result.Errors)
                    errors += $"{error.Description}, ";
                errors = errors.Trim(new[] { ',', ' ' });
                return new AuthModel { Message = errors, IsAuthenticated = false };
            }
            // Assign the role
            var role = GetRole(model.Role.ToLower());
            await _userManager.AddToRoleAsync(user, role);
            // Making the token
            var jwtSecurityToken = await CreateJwtToken(user);

            var refreshToken = GenerateRefreshToken();
            user.RefreshTokens?.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            // Returning the AuthModel containing the token created
            return new AuthModel
            {
                Message = "User has been created successfully.",
                Email = user.Email,
                UserId = user.Id,
                IsAuthenticated = true,
                Roles = await _userManager.GetRolesAsync(user),
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName = user.UserName,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.ExpiresOn
            };
        }

        public async Task<AuthModel> LoginAsync(LoginModel model)
        {
            //var user = await _userManager.FindByEmailAsync(model.Email);
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens).AsSplitQuery()
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return new AuthModel { Message = "Email or password is incorrect!" };

            var authModel = await GetTokenAsync(user);

            return authModel;
        }

        public async Task<AuthModel> RequestTokenAsync(TokenRequestModel model)
        {
            //var user = await _userManager.FindByEmailAsync(model.Email);
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens).AsSplitQuery()
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return new AuthModel { Message = "Email or password is incorrect!" };

            var authModel = await GetTokenAsync(user);

            return authModel;
        }

        public async Task<AuthModel> GetTokenAsync(ApplicationUser user)
        {
            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            var authModel = new AuthModel
            {
                Message = "Token has been generated successfully.",
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email,
                UserName = user.UserName,
                UserId = user.Id,
                Roles = rolesList.ToList(),
            };

            if (user.RefreshTokens.Any(t => t.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                authModel.RefreshToken = activeRefreshToken.Token;
                authModel.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                authModel.RefreshToken = refreshToken.Token;
                authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }

            return authModel;
        }

        public async Task<AuthModel> RefreshTokenAsync(string refreshToken)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));

            if (user == null)
                return new AuthModel { Message = "Invalid token." };

            var currentRefreshToken = user.RefreshTokens.First(t => t.Token == refreshToken);

            if (!currentRefreshToken.IsActive)
                return new AuthModel { Message = "Inactive token." };

            currentRefreshToken.RevokedOn = DateTime.UtcNow;
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            var jwtAccesToken = await CreateJwtToken(user);

            return new AuthModel
            {
                Email = user.Email,
                IsAuthenticated = true,
                Roles = await _userManager.GetRolesAsync(user),
                UserId = user.Id,
                UserName = user.UserName,
                Message = "Successfully generated new access token.",
                Token = new JwtSecurityTokenHandler().WriteToken(jwtAccesToken),
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiration = newRefreshToken.ExpiresOn
            };
        }

        public async Task<AuthModel> RevokeAndGenerate(string refreshToken)
        {
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken && t.ExpiresOn > DateTime.UtcNow));

            if (user == null)
                return new AuthModel { Message = "Invalid token." };

            var currentRefreshToken = user.RefreshTokens.First(t => t.Token == refreshToken);

            if (!currentRefreshToken.IsActive)
                return new AuthModel { Message = "Inactive token." };

            currentRefreshToken.RevokedOn = DateTime.UtcNow;
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            return new AuthModel
            {
                Email = user.Email,
                UserId = user.Id,
                UserName = user.UserName,
                Roles = await _userManager.GetRolesAsync(user),
                IsAuthenticated = true,
                Message = "Refresh token has been revoked and auto generated.",
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiration = newRefreshToken.ExpiresOn
            };
        }
        #endregion

        #region Helper methods
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(roleClaims)
            .Union(userClaims);

            var symmerticSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmerticSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
                return false;

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
                return false;

            refreshToken.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            return true;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(10),
                CreatedOn = DateTime.UtcNow
            };
        }
        public string GetRole(string roleName)
        {
            return roleName switch
            {
                "client" => Constants.Client,
                "employee" => Constants.Employee,
                "admin" => Constants.Admin,
                _ => "Client"
            };
        }
        #endregion
    }
}