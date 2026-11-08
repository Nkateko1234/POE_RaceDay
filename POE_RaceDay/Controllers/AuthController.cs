using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POE_RaceDay.Database;

namespace POE_RaceDay.Controllers
{//start of namespace
    public enum UserRole
    {
        Organiser,
        Participant
    }

    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {//start of AuthController
        [HttpPost("register")]
        public IActionResult Register( string firstName, string lastName, string email, string password, UserRole role)
        {//start of register
            store_user user = new store_user();

            string result = user.register_user(firstName,lastName, email, password, role.ToString()
        );

            if(result.StartsWith("Registration successful."))
    {
                return Ok(new
                {
                    message = result
                });
            }

            // Return an error if registration failed
            return BadRequest(new
            {
                message = result
            });
        }//end of register

        [HttpPost("login")]
        public IActionResult Login(string email, string password)
        {//start of login
            login_user user = new login_user();

            int userID;
            string role;

            string result = user.login(email,password,out userID,out role);

            if (result != "Login successful.")
            {
                return Unauthorized(new
                {
                    message = result
                });
            }

            HttpContext.Session.SetInt32("UserID", userID);
            HttpContext.Session.SetString("Role", role);

            return Ok(new
            {
                message = result,
                userID = userID,
                role = role
            });
        }//end of login

        [HttpPost("logout")]
        public string Logout()
        {//start of log out
            HttpContext.Session.Clear();

            return "Logout successful.";
        }//end of logout
    }//end of AuthController
}//end of namespace
