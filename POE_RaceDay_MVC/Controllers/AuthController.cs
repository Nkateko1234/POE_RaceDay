using Microsoft.AspNetCore.Mvc;
using POE_RaceDay_MVC.Models;
using System.Text.Json;

namespace POE_RaceDay_MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public AuthController(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        private HttpClient CreateApiClient()
        {
            HttpClient client = _clientFactory.CreateClient("RaceDayAPI");

            client.BaseAddress = new Uri( _configuration["ApiSettings:BaseUrl"]!);

            string? apiCookie = HttpContext.Session.GetString("ApiCookie");

            if (!string.IsNullOrEmpty(apiCookie))
            {
                client.DefaultRequestHeaders.Add( "Cookie",apiCookie);
            }

            return client;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login( LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            HttpClient client = CreateApiClient();

            string url = "api/Auth/login?email=" + Uri.EscapeDataString(model.Email) + "&password=" + Uri.EscapeDataString(model.Password);

            HttpResponseMessage response = await client.PostAsync(url, null);

            string responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Invalid email or password.";

                return View(model);
            }

            // Save the API session cookie for future API requests.
            if (response.Headers.TryGetValues( "Set-Cookie", out var cookies))
            {
                string cookieValue = string.Join( "; ", cookies.Select(cookie => cookie.Split(';')[0])
                );

                HttpContext.Session.SetString( "ApiCookie",cookieValue);
            }

            using JsonDocument document =JsonDocument.Parse(responseText);

            JsonElement data = document.RootElement;

            int userID = data.GetProperty("userID").GetInt32();

            string role = data.GetProperty("role").GetString()!;

            HttpContext.Session.SetInt32( "UserID", userID);

            HttpContext.Session.SetString( "Role", role);

            if (role == "Organiser")
            {
                return RedirectToAction( "Dashboard", "Organiser");
            }

            return RedirectToAction( "Dashboard", "Participant");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register( RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            HttpClient client = CreateApiClient();

            string url =
                "api/Auth/register?firstName=" +
                Uri.EscapeDataString(model.FirstName) +
                "&lastName=" +
                Uri.EscapeDataString(model.LastName) +
                "&email=" +
                Uri.EscapeDataString(model.Email) +
                "&password=" +
                Uri.EscapeDataString(model.Password) +
                "&role=" +
                Uri.EscapeDataString(model.Role);

            HttpResponseMessage response = await client.PostAsync(url, null);

            string result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    using JsonDocument document = JsonDocument.Parse(result);

                    ViewBag.Error = document.RootElement .GetProperty("message").GetString();
                }
                catch
                {
                    ViewBag.Error = result;
                }
                
                return View(model);
            }

            ViewBag.Success = "Registration successful. Please log in.";

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                HttpClient client = CreateApiClient();

                await client.PostAsync( "api/Auth/logout", null);
            }
            catch
            {
                // Still clear the MVC session if the API is unavailable.
            }

            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }

    }
}
