using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers
{
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}
