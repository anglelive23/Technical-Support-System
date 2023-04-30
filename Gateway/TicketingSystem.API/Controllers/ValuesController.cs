namespace Gateway.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValuesController : AuthBaseModel
    {

        public ValuesController(IAuthService authService) : base(authService) { }

        [HttpGet]
        public IActionResult Get()
        {
            #region Revoke And Generate
            var authModel = RevokeAndGenerate();
            if (!authModel.IsAuthenticated)
                return Unauthorized(authModel.Message);
            #endregion

            return Ok("Welcome from secured controller");
        }
    }
}
