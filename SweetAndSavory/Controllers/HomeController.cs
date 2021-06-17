using Microsoft.AspNetCore.Mvc;

namespace SweetAndSavory.Controllers
{
    public class HomeController : Controller
    {

      [HttpGet("/")]
      public ActionResult Index()
      {
        return View();
      }

    }
}