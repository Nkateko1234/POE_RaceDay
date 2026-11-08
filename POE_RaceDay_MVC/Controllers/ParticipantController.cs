using Microsoft.AspNetCore.Mvc;

namespace POE_RaceDay_MVC.Controllers
{
    public class ParticipantController : Controller
    {
        public IActionResult Dashboard()
        {
            int? userID = HttpContext.Session.GetInt32("UserID");

            string? role = HttpContext.Session.GetString("Role");

            if (userID == null || role != "Participant")
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.UserID = userID;
            return View();
        }
    }
}
