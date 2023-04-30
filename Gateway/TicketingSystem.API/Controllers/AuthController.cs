namespace Gateway.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : AuthBaseModel
    {
        #region Constructors
        public AuthController(IAuthService authService) : base(authService) { }
        #endregion

        #region Authentication Endpoints
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            try
            {
                Log.Information("Statring controller Auth action RegisterAsync.");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _authService.RegisterAsync(model);
                if (!result.IsAuthenticated)
                    return BadRequest(result.Message);

                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

                Log.Information("returning register result to the caller.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
        {
            try
            {
                Log.Information("Statring controller Auth action LoginAsync.");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _authService.LoginAsync(model);
                if (!result.IsAuthenticated)
                    return BadRequest(result.Message);

                #region RefreshToken
                if (!string.IsNullOrEmpty(result.Token))
                    SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
                #endregion

                Log.Information("returning login result to the caller.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestModel model)
        {
            try
            {
                Log.Information("Statring controller Auth action TokenRequestAsync.");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _authService.RequestTokenAsync(model);
                if (!result.IsAuthenticated)
                    return BadRequest(result.Message);

                Log.Information("returning token result to the caller.");

                #region RefreshToken
                if (!string.IsNullOrEmpty(result.Token))
                    SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
                #endregion

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];

                var result = await _authService.RefreshTokenAsync(refreshToken);

                if (!result.IsAuthenticated)
                    return BadRequest(result);

                #region RefreshToken
                if (!string.IsNullOrEmpty(result.Token))
                    SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
                #endregion

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("revokeToken")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeToken model)
        {
            try
            {
                var token = model.Token ?? Request.Cookies["refreshToken"];

                if (string.IsNullOrEmpty(token))
                    return BadRequest("Token is required!");

                var result = await _authService.RevokeTokenAsync(token);

                if (!result)
                    return BadRequest("Token is invalid!");

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        #endregion
    }
}
