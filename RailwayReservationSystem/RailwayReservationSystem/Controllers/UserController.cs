using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RailwayReservationSystem.Data;
using RailwayReservationSystem.Data.Repository;
using RailwayReservationSystem.Models;

using System.Threading.Tasks;

namespace RailwayReservationSystem.Controllers
{
    [Route("User")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _repo;
        private readonly EmailRepository _mail;

        public UserController(IConfiguration configuration, IUserRepository repo, EmailRepository mail)
        {
            _configuration = configuration;
            _repo = repo;
            _mail = mail;
        }

        #region "Login Functionality"
        [HttpPost]
        [Route("Login")]
        public ActionResult Login([FromBody] Login login)
        {
            User user = _repo.CheckUser(login);
            if(user == null)
                return NotFound(new { msg = "User Not Found..."});

            var token = _repo.GenerateJWT(user);
            return Ok(token);

        }

        
        #endregion

        #region "Register Functionality"
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register([FromBody]Register reg)
        {
            if (ModelState.IsValid)
            {
                if (_repo.CheckEmail(reg))
                {
                    User user = _repo.AddUser(reg);

                    MailDetails mailDetails = new MailDetails
                    {
                        To = user.Email,
                        Subject = "Account Successfully Created"
                    };

                    await _mail.SendEmail(mail: mailDetails, userId: user.UserId);

                    return CreatedAtAction("Register", user);
                }
                else
                {
                    return Conflict(new { msg = "Email Already Exists..." });
                }
            }
            else
            {
                return ValidationProblem("Check all fields");
            }
        }
        #endregion
    }
}
