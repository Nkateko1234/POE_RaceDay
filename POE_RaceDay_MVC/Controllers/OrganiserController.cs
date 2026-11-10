using Microsoft.AspNetCore.Mvc;
using POE_RaceDay_MVC.Models;
using System.Net.Http;
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


        [HttpGet]
        public async Task<IActionResult> Enrollments(int eventID)
        {
            // Check that the user is an Organiser
            if (!IsOrganiser())
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                HttpClient client = CreateApiClient();

                // Request enrolments from the API
                HttpResponseMessage response =
                    await client.GetAsync("api/Enrollments/event/" + eventID);

                // Read the API response
                string json =
                    await response.Content.ReadAsStringAsync();

                // Check whether the API request was successful
                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Error =
                        "API Error: " +
                        (int)response.StatusCode +
                        " - " +
                        response.StatusCode +
                        ". Details: " +
                        json;

                    return View(new List<EnrollmentViewModel>());
                }

                // Convert API JSON into MVC models
                List<EnrollmentViewModel> enrolments =
                    JsonSerializer.Deserialize<List<EnrollmentViewModel>>(
                        json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }) ?? new List<EnrollmentViewModel>();

                return View(enrolments);
            }
            catch (Exception ex)
            {
                ViewBag.Error =
                    "Error loading enrolments: " + ex.Message;

                return View(new List<EnrollmentViewModel>());
            }
        }
        // DISPLAY CAPTURE RESULT FORM
        [HttpGet]
        public IActionResult CaptureResult(int enrollmentID)
        {
            if (!IsOrganiser())
            {
                return RedirectToAction("Login", "Auth");
            }

            var model = new CaptureResultViewModel
            {
                EnrollmentID = enrollmentID
            };

            return View(model);
        }


        // SEND RESULT TO API
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CaptureResult(
            CaptureResultViewModel model)
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
                    "api/Results?enrollmentID=" + model.EnrollmentID +
                    "&finishTime=" + Uri.EscapeDataString(
                        model.FinishTime.ToString(@"hh\:mm\:ss")) +
                    "&position=" + model.Position;

                HttpResponseMessage response =
                    await client.PostAsync(url, null);

                string result =
                    await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Error = result;
                    return View(model);
                }

                TempData["Success"] = "Result saved successfully.";

                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error saving result: " + ex.Message;
                return View(model);
            }
        }
        // DISPLAY CATEGORIES
        [HttpGet]
        public async Task<IActionResult> Categories()
        {
            if (!IsOrganiser())
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                HttpClient client = CreateApiClient();

                // Get all categories from the API
                HttpResponseMessage categoryResponse =
                    await client.GetAsync("api/Categories");

                // Get events for the category dropdown
                HttpResponseMessage eventResponse =
                    await client.GetAsync("api/Events");

                if (!categoryResponse.IsSuccessStatusCode)
                {
                    ViewBag.Error = "Could not load categories.";
                    return View(new List<CategoryViewModel>());
                }

                string categoryJson =
                    await categoryResponse.Content.ReadAsStringAsync();

                List<CategoryViewModel> categories =
                    JsonSerializer.Deserialize<List<CategoryViewModel>>(
                        categoryJson,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }) ?? new List<CategoryViewModel>();

                if (eventResponse.IsSuccessStatusCode)
                {
                    string eventJson =
                        await eventResponse.Content.ReadAsStringAsync();

                    List<EventViewModel> events =
                        JsonSerializer.Deserialize<List<EventViewModel>>(
                            eventJson,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            }) ?? new List<EventViewModel>();

                    ViewBag.Events = events;
                }
                else
                {
                    ViewBag.Events = new List<EventViewModel>();
                    ViewBag.Error = "Categories loaded, but events could not be loaded.";
                }

                return View(categories);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error loading categories: " + ex.Message;
                ViewBag.Events = new List<EventViewModel>();

                return View(new List<CategoryViewModel>());
            }
        }


        // CREATE A CATEGORY
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(CategoryViewModel model)
        {
            if (!IsOrganiser())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (model.EventID <= 0 ||
                string.IsNullOrWhiteSpace(model.CategoryName) ||
                string.IsNullOrWhiteSpace(model.Distance) ||
                model.EntryFee < 0)
            {
                TempData["Error"] = "Please enter valid category details.";
                return RedirectToAction("Categories");
            }

            try
            {
                HttpClient client = CreateApiClient();

                string url =
                    "api/Categories?eventID=" + model.EventID +
                    "&categoryName=" + Uri.EscapeDataString(model.CategoryName) +
                    "&distance=" + Uri.EscapeDataString(model.Distance) +
                    "&entryFee=" + model.EntryFee.ToString(
                        System.Globalization.CultureInfo.InvariantCulture);

                HttpResponseMessage response =
                    await client.PostAsync(url, null);

                string result =
                    await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode ||
                    !result.Contains("Category created successfully."))
                {
                    TempData["Error"] = result;
                    return RedirectToAction("Categories");
                }

                TempData["Success"] = "Category created successfully.";
                return RedirectToAction("Categories");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error creating category: " + ex.Message;
                return RedirectToAction("Categories");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Results()
        {
            if (!IsOrganiser())
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                HttpClient client = CreateApiClient();

                HttpResponseMessage response =
                    await client.GetAsync("api/Results/organiser-results");

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Could not retrieve race results.";

                    return View(new List<ResultViewModel>());
                }

                string json = await response.Content.ReadAsStringAsync();

                List<ResultViewModel> results =
                    JsonSerializer.Deserialize<List<ResultViewModel>>(
                        json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        })
                    ?? new List<ResultViewModel>();

                return View(results);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading results: " + ex.Message;

                return View(new List<ResultViewModel>());
            }
        }
    }
}
