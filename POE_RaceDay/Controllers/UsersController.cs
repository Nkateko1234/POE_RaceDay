using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POE_RaceDay.Database;

namespace POE_RaceDay.Controllers
{//start of amespace
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {//end of class

        //HttpGet to get profile
        [HttpGet("profile")]
        public string GetProfile()
        {//start of GetProfile
            int? userID = HttpContext.Session.GetInt32("UserID");

            if (userID == null)
            {
                return "You must be logged in.";
            }

            get_user user = new get_user();

            return user.get_info(userID.Value);
        }//end of GetProfile

        //HttpPut to updateProfile
        [HttpPut("profile")]
        public string UpdateProfile( string firstName, string lastName, string email)
        {//start of UpdateProfile
            int? userID = HttpContext.Session.GetInt32("UserID");

            if (userID == null)
            {
                return "You must be logged in.";
            }

            update_user user =
                new update_user();

            return user.update_info( userID.Value, firstName, lastName, email);
        }//end of UpdateProfile
    }//end of class
}//end of amespace
