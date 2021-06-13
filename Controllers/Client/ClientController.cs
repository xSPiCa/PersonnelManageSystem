using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PersonnelManageSystem.Controllers.Client
{
    public class ClientController : Controller
    {
        
        [Route("/")]
        public IActionResult Index()
        {
            return View("Index");
        }
    }
}