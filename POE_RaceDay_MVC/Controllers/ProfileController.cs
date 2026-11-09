using Microsoft.AspNetCore.Mvc;
using POE_RaceDay_MVC.Models;

namespace POE_RaceDay_MVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public ProfileController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        // Display profile page
        [HttpGet]
        public IActionResult Index()
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            string? role = HttpContext.Session.GetString("Role");

            if (userID == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            ProfileViewModel model = new ProfileViewModel();

            model.UserID = userID.Value;
            model.Role = role ?? "";

            // Load saved profile picture path
            model.ProfileImage =
                HttpContext.Session.GetString("ProfileImage")
                ?? "/images/profiles/default-profile.png";

            return View(model);
        }

        // Upload profile picture
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(ProfileViewModel model)
        {
            int? userID = HttpContext.Session.GetInt32("UserID");

            if (userID == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (model.ImageFile == null ||
                model.ImageFile.Length == 0)
            {
                TempData["Error"] = "Please select an image.";
                return RedirectToAction("Index");
            }

            // Check file type
            string extension =
                Path.GetExtension(model.ImageFile.FileName).ToLowerInvariant();

            string[] allowedExtensions =
                { ".jpg", ".jpeg", ".png", ".webp" };

            if (!allowedExtensions.Contains(extension))
            {
                TempData["Error"] =
                    "Only JPG, JPEG, PNG and WEBP images are allowed.";

                return RedirectToAction("Index");
            }

            // Check file size (5 MB maximum)
            if (model.ImageFile.Length > 5 * 1024 * 1024)
            {
                TempData["Error"] =
                    "Image must be smaller than 5 MB.";

                return RedirectToAction("Index");
            }

            // Create profiles folder if it doesn't exist
            string folderPath = Path.Combine(
                _environment.WebRootPath,
                "images",
                "profiles"
            );

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Generate a unique file name
            string fileName =
                $"profile_{userID.Value}_{Guid.NewGuid()}{extension}";

            string filePath = Path.Combine(folderPath, fileName);

            // Save image
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await model.ImageFile.CopyToAsync(stream);
            }

            // Save image path in the current session
            string imageUrl = "/images/profiles/" + fileName;

            HttpContext.Session.SetString("ProfileImage", imageUrl);

            TempData["Success"] = "Profile picture uploaded successfully!";

            return RedirectToAction("Index");
        }
    }
}
