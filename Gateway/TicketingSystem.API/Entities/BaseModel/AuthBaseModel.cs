

namespace Gateway.API.Entities.BaseModel
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthBaseModel : ODataController
    {
        #region Services
        protected readonly IAuthService _authService;
        protected readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Constructors
        public AuthBaseModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public AuthBaseModel(IAuthService authService)
        {
            _authService = authService;
        }
        #endregion

        #region Cookie
        [ApiExplorerSettings(IgnoreApi = true)]
        protected void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,
                Expires = expires.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None,
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
        #endregion

        #region Revoke And Generate
        [ApiExplorerSettings(IgnoreApi = true)]
        public AuthModel RevokeAndGenerate()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var result = _authService.RevokeAndGenerate(refreshToken).Result;
            if (!result.IsAuthenticated)
                return new AuthModel { Message = result.Message };
            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
            //SetRefreshTokenInResponseHeaders(result.RefreshToken);
            return result;
        }
        #endregion

        #region Maybe --> Add refreshToken in Response Header
        [ApiExplorerSettings(IgnoreApi = true)]
        protected void SetRefreshTokenInResponseHeaders(string refreshToken)
        {
            Response.Headers.Append("refreshToken", refreshToken);
        }
        #endregion

    }
}
