using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PersonnelManageSystem.Controllers.Client.Api
{
    [Route("Api/IndividualInfo")]
    [ApiController]
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class IndividualInfoController : Controller
    {
        
    }
}