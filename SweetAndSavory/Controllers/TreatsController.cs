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
  public class TreatsController : Controller
  {
    private readonly SweetAndSavoryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public TreatsController(UserManager<ApplicationUser> userManager, SweetAndSavoryContext db)
    {
      _db = db;
      _userManager = userManager;
    }

    public ActionResult Index()
    {
      var treats = _db.Treats
        .Include(treat => treat.JoinEntities)
        .ThenInclude(join => join.Flavor)
        .ToList();
      return View(treats);
    }

    public ActionResult Create()
    {
      var flavors = _db.Flavors.ToList();
      ViewBag.FlavorId = new SelectList(flavors, "FlavorId", "Name");
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Treat treat, int flavorId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      treat.User = currentUser;
      _db.Treats.Add(treat);
      _db.SaveChanges();
      if (flavorId != 0)
      {
        _db.FlavorTreats.Add(new FlavorTreat { FlavorId = flavorId, TreatId = treat.TreatId});
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Edit(int id)
    {
      Treat thisTreat = _db.Treats
        .Include(treat => treat.JoinEntities)
        .ThenInclude(join => join.Flavor)
        .FirstOrDefault(treat => treat.TreatId == id);
      List<Flavor> flavors = _db.Flavors.ToList();
      ViewBag.FlavorId = new SelectList(flavors, "FlavorId", "Name");
      return View(thisTreat);
    }

    [HttpPost]
    public ActionResult Edit(Treat treat, int flavorId)
    {
      if (flavorId != 0)
      {
        _db.FlavorTreats.Add(new FlavorTreat {TreatId = treat.TreatId, FlavorId = flavorId});
      }
      _db.Entry(treat).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Edit", new {id = treat.TreatId});
    }
    [AllowAnonymous]
    public ActionResult Details(int id)
    {
      var thisTreat = _db.Treats
        .Include(treat => treat.JoinEntities)
        .ThenInclude(join => join.Flavor)
        .FirstOrDefault(treat => treat.TreatId == id);
      return View(thisTreat);
    }

    public ActionResult Delete(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      return View(thisTreat);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      _db.Treats.Remove(thisTreat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult AddFlavor(Treat treat, int flavorId)
    {
      if (flavorId != 0)
      {
        _db.FlavorTreats.Add(new FlavorTreat {TreatId = treat.TreatId, FlavorId = flavorId});
      }
      _db.SaveChanges();
      return RedirectToAction("Edit", new {id = treat.TreatId});
    }

    [HttpPost]
    public ActionResult DeleteFlavor(int joinId)
    {
      FlavorTreat joinEntry = _db.FlavorTreats.FirstOrDefault(entry => entry.FlavorTreatId == joinId);
      int treatId = joinEntry.TreatId;
      _db.FlavorTreats.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Edit", new { id = treatId});
    }


    
  }
}