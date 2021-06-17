using Microsoft.AspNetCore.Mvc;
using SweetAndSavory.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace SweetAndSavory.Controllers
{
  [Authorize]
  public class FlavorsController : Controller
  {
    private readonly SweetAndSavoryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public FlavorsController(UserManager<ApplicationUser> userManager, SweetAndSavoryContext db)
    {
      _db = db;
      _userManager = userManager;
    }

    public ActionResult Index()
    {
      var flavors = _db.Flavors.ToList();
      return View(flavors);
    }
  }
}