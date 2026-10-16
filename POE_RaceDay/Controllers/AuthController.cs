using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POE_RaceDay.Database;

namespace POE_RaceDay.Controllers
{
    public enum UserRole
    {
        Organiser,
        Participant
    }

    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        public string Register(
            string firstName,
            string lastName,
            string email,
            string password,
            UserRole role)
        {
            store_user user = new store_user();

            return user.register_user(
                firstName,
                lastName,
                email,
                password,
                role.ToString());
        }

        [HttpPost("login")]
        public string Login(
            string email,
            string password)
        {
            login_user user = new login_user();

            int userID;
            string role;

            string result =
                user.login(
                    email,
                    password,
                    out userID,
                    out role);

            if (result == "Login successful.")
            {
                HttpContext.Session.SetInt32("UserID", userID);
                HttpContext.Session.SetString("Role", role);
            }

            return result;
        }

        [HttpPost("logout")]
        public string Logout()
        {
            HttpContext.Session.Clear();

            return "Logout successful.";
        }
    }
}
