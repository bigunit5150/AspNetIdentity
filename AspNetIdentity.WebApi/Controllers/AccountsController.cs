using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AspNetIdentity.WebApi.Infrastructure;
using AspNetIdentity.WebApi.Models;

namespace AspNetIdentity.WebApi.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountsController : BaseApiController
    {
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            return Ok(this.AppUserManager.Users.ToList().Select(u => this.TheModelFactory.Create(u)));
        }

        [Route("user/{id:guid}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUser(string Id)
        {
            var user = await this.AppUserManager.FindByIdAsync(Id);

            if (user != null)
                return Ok(this.TheModelFactory.Create(user));

            return NotFound();
        }

        [Route("user/{username}")]
        public async Task<IHttpActionResult> GetUserByName(string username)
        {
            var user = await this.AppUserManager.FindByNameAsync(username);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();
        }

        [Route("create")]
        public async Task<IHttpActionResult> CreateUser(CreateUserBindingModel createUserModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = new ApplicationUser
            {
                UserName = createUserModel.UserName,
                Email = createUserModel.Email,
                FirstName = createUserModel.FirstName,
                LastName = createUserModel.LastName,
                Level = 3,
                JoinDate = DateTime.Now.Date
            };

            var addUserResult = await this.AppUserManager.CreateAsync(user, createUserModel.Password);
            if (!addUserResult.Succeeded)
                return GetErrorResult(addUserResult);


            string code = await this.AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            var callbackUrl = new Uri(Url.Link("ConfirmEmailroute", new {userId = user.Id, code = code}));
            await
                this.AppUserManager.SendEmailAsync(user.Id, "Confirm your account",
                    "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

            Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));

            return Created(locationHeader, TheModelFactory.Create(user));
        }

        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId = "", string code = "")
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }

            var result = await this.AppUserManager.ConfirmEmailAsync(userId, code);
            return result.Succeeded ? Ok() : GetErrorResult(result);
        }
    }
}