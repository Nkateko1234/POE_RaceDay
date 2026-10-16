using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using POE_RaceDay.Controllers;

namespace TestProject1
{
    public class UnitTest1
    {
        // =====================================================
        // AUTH CONTROLLER TESTS
        // =====================================================

        [Fact]
        public void Logout_ReturnsSuccessfulMessage()
        {
            // Arrange
            AuthController controller =
                new AuthController();

            DefaultHttpContext context =
                CreateHttpContextWithSession();

            controller.ControllerContext =
                new ControllerContext
                {
                    HttpContext = context
                };

            // Act
            string result =
                controller.Logout();

            // Assert
            Assert.Equal(
                "Logout successful.",
                result);
        }


        // =====================================================
        // EVENTS CONTROLLER - UNAUTHENTICATED TESTS
        // =====================================================

        [Fact]
        public void CreateEvent_WithoutLogin_IsRejected()
        {
            // Arrange
            EventsController controller =
                CreateController();

            // Act
            string result =
                controller.CreateEvent(
                    "Marathon",
                    "Cape Town Race",
                    DateTime.Now,
                    "Cape Town",
                    "Annual running event");

            // Assert
            Assert.Equal(
                "You must be logged in.",
                result);
        }


        [Fact]
        public void UpdateEvent_WithoutLogin_IsRejected()
        {
            // Arrange
            EventsController controller =
                CreateController();

            // Act
            string result =
                controller.UpdateEvent(
                    1,
                    "Marathon",
                    "Updated Race",
                    DateTime.Now,
                    "Cape Town",
                    "Updated event");

            // Assert
            Assert.Equal(
                "You must be logged in.",
                result);
        }


        [Fact]
        public void DeleteEvent_WithoutLogin_IsRejected()
        {
            // Arrange
            EventsController controller =
                CreateController();

            // Act
            string result =
                controller.DeleteEvent(1);

            // Assert
            Assert.Equal(
                "You must be logged in.",
                result);
        }


        // =====================================================
        // EVENTS CONTROLLER - ROLE TESTS
        // =====================================================

        [Fact]
        public void CreateEvent_Participant_IsRejected()
        {
            // Arrange
            EventsController controller =
                CreateControllerWithSession(
                    1,
                    "Participant");

            // Act
            string result =
                controller.CreateEvent(
                    "Marathon",
                    "Participant Race",
                    DateTime.Now,
                    "Cape Town",
                    "Test event");

            // Assert
            Assert.Equal(
                "Only Organisers can create events.",
                result);
        }


        [Fact]
        public void UpdateEvent_Participant_IsRejected()
        {
            // Arrange
            EventsController controller =
                CreateControllerWithSession(
                    1,
                    "Participant");

            // Act
            string result =
                controller.UpdateEvent(
                    1,
                    "Marathon",
                    "Updated Race",
                    DateTime.Now,
                    "Cape Town",
                    "Updated event");

            // Assert
            Assert.Equal(
                "Only Organisers can update events.",
                result);
        }


        [Fact]
        public void DeleteEvent_Participant_IsRejected()
        {
            // Arrange
            EventsController controller =
                CreateControllerWithSession(
                    1,
                    "Participant");

            // Act
            string result =
                controller.DeleteEvent(1);

            // Assert
            Assert.Equal(
                "Only Organisers can delete events.",
                result);
        }


        // =====================================================
        // ENROLLMENT - UNAUTHENTICATED TESTS
        // =====================================================

        [Fact]
        public void Enroll_WithoutLogin_IsRejected()
        {
            // Arrange
            EnrollmentsController controller =
                CreateEnrollmentController();

            // Act
            string result =
                controller.Enroll(1, 1);

            // Assert
            Assert.Equal(
                "You must be logged in.",
                result);
        }


        [Fact]
        public void MyEnrollments_WithoutLogin_IsRejected()
        {
            // Arrange
            EnrollmentsController controller =
                CreateEnrollmentController();

            // Act
            string result =
                controller.MyEnrollments();

            // Assert
            Assert.Equal(
                "You must be logged in.",
                result);
        }


        [Fact]
        public void EventEnrollments_WithoutLogin_IsRejected()
        {
            // Arrange
            EnrollmentsController controller =
                CreateEnrollmentController();

            // Act
            string result =
                controller.EventEnrollments(1);

            // Assert
            Assert.Equal(
                "You must be logged in.",
                result);
        }


        // =====================================================
        // ENROLLMENT - ROLE TESTS
        // =====================================================

        [Fact]
        public void Enroll_Organiser_IsRejected()
        {
            // Arrange
            EnrollmentsController controller =
                CreateEnrollmentControllerWithSession(
                    1,
                    "Organiser");

            // Act
            string result =
                controller.Enroll(1, 1);

            // Assert
            Assert.Equal(
                "Only Participants can enrol in events.",
                result);
        }


        [Fact]
        public void MyEnrollments_Organiser_IsRejected()
        {
            // Arrange
            EnrollmentsController controller =
                CreateEnrollmentControllerWithSession(
                    1,
                    "Organiser");

            // Act
            string result =
                controller.MyEnrollments();

            // Assert
            Assert.Equal(
                "Only Participants can view their enrollments.",
                result);
        }


        [Fact]
        public void EventEnrollments_Participant_IsRejected()
        {
            // Arrange
            EnrollmentsController controller =
                CreateEnrollmentControllerWithSession(
                    1,
                    "Participant");

            // Act
            string result =
                controller.EventEnrollments(1);

            // Assert
            Assert.Equal(
                "Only Organisers can view event enrollments.",
                result);
        }


        // =====================================================
        // CATEGORY TESTS
        // =====================================================

        [Fact]
        public void CreateCategory_WithoutLogin_IsRejected()
        {
            // Arrange
            CategoriesController controller =
                CreateCategoryController();

            // Act
            string result =
                controller.CreateCategory(
                    1,
                    "10km",
                    "10 km",
                    100);

            // Assert
            Assert.Equal(
                "You must be logged in.",
                result);
        }


        [Fact]
        public void CreateCategory_Participant_IsRejected()
        {
            // Arrange
            CategoriesController controller =
                CreateCategoryControllerWithSession(
                    1,
                    "Participant");

            // Act
            string result =
                controller.CreateCategory(
                    1,
                    "10km",
                    "10 km",
                    100);

            // Assert
            Assert.Equal(
                "Only Organisers can create categories.",
                result);
        }


        // =====================================================
        // RESULTS TESTS
        // =====================================================

        [Fact]
        public void AddResult_WithoutLogin_IsRejected()
        {
            // Arrange
            ResultsController controller =
                CreateResultsController();

            // Act
            string result =
                controller.AddResult(
                    1,
                    new TimeSpan(0, 45, 30),
                    1);

            // Assert
            Assert.Equal(
                "You must be logged in.",
                result);
        }


        [Fact]
        public void AddResult_Participant_IsRejected()
        {
            // Arrange
            ResultsController controller =
                CreateResultsControllerWithSession(
                    1,
                    "Participant");

            // Act
            string result =
                controller.AddResult(
                    1,
                    new TimeSpan(0, 45, 30),
                    1);

            // Assert
            Assert.Equal(
                "Only Organisers can capture results.",
                result);
        }


        [Fact]
        public void MyResults_WithoutLogin_IsRejected()
        {
            // Arrange
            ResultsController controller =
                CreateResultsController();

            // Act
            string result =
                controller.MyResults();

            // Assert
            Assert.Equal(
                "You must be logged in.",
                result);
        }


        [Fact]
        public void MyResults_Organiser_IsRejected()
        {
            // Arrange
            ResultsController controller =
                CreateResultsControllerWithSession(
                    1,
                    "Organiser");

            // Act
            string result =
                controller.MyResults();

            // Assert
            Assert.Equal(
                "Only Participants can view their results.",
                result);
        }


        // =====================================================
        // USER PROFILE TESTS
        // =====================================================

        [Fact]
        public void GetProfile_WithoutLogin_IsRejected()
        {
            // Arrange
            UsersController controller =
                CreateUsersController();

            // Act
            string result =
                controller.GetProfile();

            // Assert
            Assert.Equal(
                "You must be logged in.",
                result);
        }


        [Fact]
        public void UpdateProfile_WithoutLogin_IsRejected()
        {
            // Arrange
            UsersController controller =
                CreateUsersController();

            // Act
            string result =
                controller.UpdateProfile(
                    "John",
                    "Smith",
                    "john@test.com");

            // Assert
            Assert.Equal(
                "You must be logged in.",
                result);
        }


        // =====================================================
        // HTTP CONTEXT + SESSION
        // =====================================================

        private DefaultHttpContext CreateHttpContextWithSession()
        {
            DefaultHttpContext context =
                new DefaultHttpContext();

            context.Features.Set<ISessionFeature>(
                new SessionFeature
                {
                    Session = new TestSession()
                });

            return context;
        }


        // =====================================================
        // EVENTS CONTROLLER
        // =====================================================

        private EventsController CreateController()
        {
            EventsController controller =
                new EventsController();

            DefaultHttpContext context =
                CreateHttpContextWithSession();

            controller.ControllerContext =
                new ControllerContext
                {
                    HttpContext = context
                };

            return controller;
        }


        private EventsController CreateControllerWithSession(
            int userID,
            string role)
        {
            EventsController controller =
                CreateController();

            contextSession(
                controller.HttpContext,
                userID,
                role);

            return controller;
        }


        // =====================================================
        // ENROLLMENTS CONTROLLER
        // =====================================================

        private EnrollmentsController CreateEnrollmentController()
        {
            EnrollmentsController controller =
                new EnrollmentsController();

            DefaultHttpContext context =
                CreateHttpContextWithSession();

            controller.ControllerContext =
                new ControllerContext
                {
                    HttpContext = context
                };

            return controller;
        }


        private EnrollmentsController
            CreateEnrollmentControllerWithSession(
                int userID,
                string role)
        {
            EnrollmentsController controller =
                CreateEnrollmentController();

            contextSession(
                controller.HttpContext,
                userID,
                role);

            return controller;
        }


        // =====================================================
        // CATEGORIES CONTROLLER
        // =====================================================

        private CategoriesController CreateCategoryController()
        {
            CategoriesController controller =
                new CategoriesController();

            DefaultHttpContext context =
                CreateHttpContextWithSession();

            controller.ControllerContext =
                new ControllerContext
                {
                    HttpContext = context
                };

            return controller;
        }


        private CategoriesController
            CreateCategoryControllerWithSession(
                int userID,
                string role)
        {
            CategoriesController controller =
                CreateCategoryController();

            contextSession(
                controller.HttpContext,
                userID,
                role);

            return controller;
        }


        // =====================================================
        // RESULTS CONTROLLER
        // =====================================================

        private ResultsController CreateResultsController()
        {
            ResultsController controller =
                new ResultsController();

            DefaultHttpContext context =
                CreateHttpContextWithSession();

            controller.ControllerContext =
                new ControllerContext
                {
                    HttpContext = context
                };

            return controller;
        }


        private ResultsController
            CreateResultsControllerWithSession(
                int userID,
                string role)
        {
            ResultsController controller =
                CreateResultsController();

            contextSession(
                controller.HttpContext,
                userID,
                role);

            return controller;
        }


        // =====================================================
        // USERS CONTROLLER
        // =====================================================

        private UsersController CreateUsersController()
        {
            UsersController controller =
                new UsersController();

            DefaultHttpContext context =
                CreateHttpContextWithSession();

            controller.ControllerContext =
                new ControllerContext
                {
                    HttpContext = context
                };

            return controller;
        }


        // =====================================================
        // SET USER SESSION
        // =====================================================

        private void contextSession(
            HttpContext context,
            int userID,
            string role)
        {
            context.Session.SetInt32(
                "UserID",
                userID);

            context.Session.SetString(
                "Role",
                role);
        }
    }


    // =========================================================
    // TEST SESSION
    // =========================================================

    public class TestSession : ISession
    {
        private readonly Dictionary<string, byte[]> _session =
            new Dictionary<string, byte[]>();


        public bool IsAvailable
        {
            get
            {
                return true;
            }
        }


        public string Id
        {
            get
            {
                return "TestSession";
            }
        }


        public IEnumerable<string> Keys
        {
            get
            {
                return _session.Keys;
            }
        }


        public void Clear()
        {
            _session.Clear();
        }


        public Task CommitAsync(
            CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }


        public Task LoadAsync(
            CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }


        public void Remove(string key)
        {
            _session.Remove(key);
        }


        public void Set(
            string key,
            byte[] value)
        {
            _session[key] = value;
        }


        public bool TryGetValue(
            string key,
            out byte[] value)
        {
            return _session.TryGetValue(
                key,
                out value);
        }
    }
}