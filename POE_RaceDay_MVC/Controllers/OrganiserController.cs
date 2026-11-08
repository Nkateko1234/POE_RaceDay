using Microsoft.AspNetCore.Mvc;

namespace POE_RaceDay_MVC.Controllers
{
    public class OrganiserController : Controller
    {
        public IActionResult Dashboard()
        {
            int? userID =
                HttpContext.Session.GetInt32("UserID");

            string? role =
                HttpContext.Session.GetString("Role");

            if (userID == null || role != "Organiser")
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.UserID = userID;

            return View();
        }
    }
}
