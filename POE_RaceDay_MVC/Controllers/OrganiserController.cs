using Microsoft.AspNetCore.Mvc;
using POE_RaceDay_MVC.Models;
using System.Text.Json;

namespace POE_RaceDay_MVC.Controllers
{
    public class OrganiserController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public OrganiserController(
            IHttpClientFactory clientFactory,
            IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        // Create API client and forward the API login cookie
        private HttpClient CreateApiClient()
        {
            HttpClient client =
                _clientFactory.CreateClient("RaceDayAPI");

            client.BaseAddress =
                new Uri(_configuration["ApiSettings:BaseUrl"]!);

            string? apiCookie =
                HttpContext.Session.GetString("ApiCookie");

            if (!string.IsNullOrEmpty(apiCookie))
            {
                client.DefaultRequestHeaders.Remove("Cookie");
                client.DefaultRequestHeaders.Add("Cookie", apiCookie);
            }

            return client;
        }

        // Check that the user is an Organiser
        private bool IsOrganiser()
        {
            int? userID =
                HttpContext.Session.GetInt32("UserID");

            string? role =
                HttpContext.Session.GetString("Role");

            return userID != null && role == "Organiser";
        }

        // ORGANISER DASHBOARD
        public IActionResult Dashboard()
        {
            if (!IsOrganiser())
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.UserID =
                HttpContext.Session.GetInt32("UserID");

            return View();
        }

        // DISPLAY ALL EVENTS
        [HttpGet]
        public async Task<IActionResult> Events()
        {
            if (!IsOrganiser())
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                HttpClient client = CreateApiClient();

                HttpResponseMessage response =
                    await client.GetAsync("api/Events");

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Error = "Could not load events from the API.";
                    return View(new List<EventViewModel>());
                }

                string result =
                    await response.Content.ReadAsStringAsync();

                List<EventViewModel> events =
                    JsonSerializer.Deserialize<List<EventViewModel>>(
                        result,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }) ?? new List<EventViewModel>();

                return View(events);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error loading events: " + ex.Message;
                return View(new List<EventViewModel>());
            }
        }

        // DISPLAY CREATE EVENT FORM
        [HttpGet]
        public IActionResult CreateEvent()
        {
            if (!IsOrganiser())
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        // SEND NEW EVENT TO API
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvent(EventViewModel model)
        {
            if (!IsOrganiser())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                HttpClient client = CreateApiClient();

                string url =
                    "api/Events?eventType=" + Uri.EscapeDataString(model.EventType) +
                    "&eventName=" + Uri.EscapeDataString(model.EventName) +
                    "&eventDate=" + Uri.EscapeDataString(model.EventDate.ToString("yyyy-MM-ddTHH:mm:ss")) +
                    "&eventLocation=" + Uri.EscapeDataString(model.EventLocation) +
                    "&eventDescription=" + Uri.EscapeDataString(model.EventDescription);

                HttpResponseMessage response =
                    await client.PostAsync(url, null);

                string result =
                    await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode ||
                    !result.Contains("Event created successfully."))
                {
                    ViewBag.Error = result;
                    return View(model);
                }

                TempData["Success"] = "Event created successfully.";

                return RedirectToAction("Events");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error creating event: " + ex.Message;
                return View(model);
            }
        }

        // DISPLAY EDIT EVENT FORM
        [HttpGet]
        public async Task<IActionResult> EditEvent(int id)
        {
            if (!IsOrganiser())
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                HttpClient client = CreateApiClient();

                HttpResponseMessage response =
                    await client.GetAsync("api/Events/" + id);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Event could not be found.";
                    return RedirectToAction("Events");
                }

                string result =
                    await response.Content.ReadAsStringAsync();

                EventViewModel? eventItem =
                    JsonSerializer.Deserialize<EventViewModel>(
                        result,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                if (eventItem == null)
                {
                    TempData["Error"] = "Event could not be found.";
                    return RedirectToAction("Events");
                }

                return View(eventItem);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading event: " + ex.Message;
                return RedirectToAction("Events");
            }
        }

        // SEND UPDATED EVENT TO API
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEvent(EventViewModel model)
        {
            if (!IsOrganiser())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                HttpClient client = CreateApiClient();

                string url =
                    "api/Events/" + model.EventID +
                    "?eventType=" + Uri.EscapeDataString(model.EventType) +
                    "&eventName=" + Uri.EscapeDataString(model.EventName) +
                    "&eventDate=" + Uri.EscapeDataString(model.EventDate.ToString("yyyy-MM-ddTHH:mm:ss")) +
                    "&eventLocation=" + Uri.EscapeDataString(model.EventLocation) +
                    "&eventDescription=" + Uri.EscapeDataString(model.EventDescription);

                HttpResponseMessage response =
                    await client.PutAsync(url, null);

                string result =
                    await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode ||
                    !result.Contains("Event updated successfully."))
                {
                    ViewBag.Error = result;
                    return View(model);
                }

                TempData["Success"] = "Event updated successfully.";

                return RedirectToAction("Events");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error updating event: " + ex.Message;
                return View(model);
            }
        }

        // DELETE EVENT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEvent(int eventID)
        {
            if (!IsOrganiser())
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                HttpClient client = CreateApiClient();

                HttpResponseMessage response =
                    await client.DeleteAsync("api/Events/" + eventID);

                string result =
                    await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode ||
                    !result.Contains("Event deleted successfully."))
                {
                    TempData["Error"] = result;
                }
                else
                {
                    TempData["Success"] = "Event deleted successfully.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] =
                    "Error deleting event: " + ex.Message;
            }

            return RedirectToAction("Events");
        }
    }
}
