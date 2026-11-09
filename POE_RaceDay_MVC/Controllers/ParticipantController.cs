using Microsoft.AspNetCore.Mvc;
using POE_RaceDay_MVC.Models;
using System.Text.Json;

namespace POE_RaceDay_MVC.Controllers
{
    public class ParticipantController : Controller
    {
        // API connection settings
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        // Constructor
        public ParticipantController(
            IHttpClientFactory clientFactory,
            IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        // Create API client and forward API login cookie
        private HttpClient CreateApiClient()
        {
            HttpClient client =
                _clientFactory.CreateClient("RaceDayAPI");

            client.BaseAddress =
                new Uri(_configuration["ApiSettings:BaseUrl"]!);

            // Get API cookie from MVC Session
            string? apiCookie =
                HttpContext.Session.GetString("ApiCookie");

            // Forward cookie to API
            if (!string.IsNullOrEmpty(apiCookie))
            {
                client.DefaultRequestHeaders.Remove("Cookie");
                client.DefaultRequestHeaders.Add("Cookie", apiCookie);
            }

            return client;
        }

        // Check that the user is a Participant
        private bool IsParticipant()
        {
            int? userID =
                HttpContext.Session.GetInt32("UserID");

            string? role =
                HttpContext.Session.GetString("Role");

            return userID != null && role == "Participant";
        }

        // PARTICIPANT DASHBOARD
        public IActionResult Dashboard()
        {
            if (!IsParticipant())
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.UserID =
                HttpContext.Session.GetInt32("UserID");

            return View();
        }

        // DISPLAY PARTICIPANT'S ENROLMENTS
        [HttpGet]
        public async Task<IActionResult> MyEnrollments()
        {
            if (!IsParticipant())
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                // Create API client
                HttpClient client = CreateApiClient();

                // Request the Participant's enrolments
                HttpResponseMessage response =
                    await client.GetAsync("api/Enrollments/my-enrollments");

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Error = "Could not retrieve your enrolments.";

                    return View(new List<EnrollmentViewModel>());
                }

                // Read JSON response
                string json =
                    await response.Content.ReadAsStringAsync();

                // Convert JSON into MVC models
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
                ViewBag.Error = "Error loading enrolments: " + ex.Message;

                return View(new List<EnrollmentViewModel>());
            }
        }

        // DISPLAY PARTICIPANT'S RESULTS
        [HttpGet]
        public async Task<IActionResult> MyResults()
        {
            if (!IsParticipant())
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                HttpClient client = CreateApiClient();

                HttpResponseMessage response =
                    await client.GetAsync("api/Results/my-results");

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Error = "Could not retrieve your results.";

                    return View(new List<ResultViewModel>());
                }

                string json =
                    await response.Content.ReadAsStringAsync();

                List<ResultViewModel> results =
                    JsonSerializer.Deserialize<List<ResultViewModel>>(
                        json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }) ?? new List<ResultViewModel>();

                return View(results);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error loading results: " + ex.Message;

                return View(new List<ResultViewModel>());
            }
        }
        // DISPLAY AVAILABLE EVENTS
        [HttpGet]
        public async Task<IActionResult> Events()
        {
            if (!IsParticipant())
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
                    ViewBag.Error = "Could not load available events.";
                    return View(new List<EventViewModel>());
                }

                string json =
                    await response.Content.ReadAsStringAsync();

                List<EventViewModel> events =
                    JsonSerializer.Deserialize<List<EventViewModel>>(
                        json,
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


        // DISPLAY ONE EVENT
        [HttpGet]
        public async Task<IActionResult> EventDetails(int id)
        {
            if (!IsParticipant())
            {
                return RedirectToAction("Login", "Auth");
            }

            HttpClient client = CreateApiClient();

            // Get the selected event
            HttpResponseMessage eventResponse =
                await client.GetAsync("api/Events/" + id);

            if (!eventResponse.IsSuccessStatusCode)
            {
                return NotFound();
            }

            string eventJson = await eventResponse.Content.ReadAsStringAsync();

            EventViewModel? eventDetails =
                JsonSerializer.Deserialize<EventViewModel>(
                    eventJson,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (eventDetails == null)
            {
                return NotFound();
            }

            // Get categories belonging to this event
            HttpResponseMessage categoryResponse =
                await client.GetAsync("api/Categories/event/" + id);

            List<CategoryViewModel> categories = new List<CategoryViewModel>();

            if (categoryResponse.IsSuccessStatusCode)
            {
                string categoryJson =
                    await categoryResponse.Content.ReadAsStringAsync();

                categories =
                    JsonSerializer.Deserialize<List<CategoryViewModel>>(
                        categoryJson,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        })
                    ?? new List<CategoryViewModel>();
            }

            ViewBag.Categories = categories;

            return View(eventDetails);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll(int eventID, int categoryID)
        {
            if (!IsParticipant())
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                HttpClient client = CreateApiClient();

                string url = "api/Enrollments?eventID=" + eventID
                           + "&categoryID=" + categoryID;

                HttpResponseMessage response =
                    await client.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "You have successfully enrolled in this event!";

                    return RedirectToAction("MyEnrollments");
                }

                string error = await response.Content.ReadAsStringAsync();

                TempData["Error"] = error;

                return RedirectToAction("EventDetails", new { id = eventID });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Enrollment error: " + ex.Message;

                return RedirectToAction("EventDetails", new { id = eventID });
            }
        }
    }
}
