using John.Yeung.WsecuApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace John.Yeung.WsecuApplication.Controllers
{
    public class UsersController : ApiController
    {
        private IDbContext context;

        public UsersController()
        {
            context = new WsecuEntities();
        }

        public UsersController(IDbContext _context)
        {
            context = _context;
        }

        protected override void Dispose(bool disposing)
        {
            if (context is IDisposable)
            {
                ((IDisposable)context).Dispose();
            }

            base.Dispose(disposing);
        }

        [HttpGet]
        [Route("api/Users")]
        public IEnumerable<User> Get()
        {
            return context.Users.ToList();
        }

        [HttpGet]
        [Route("api/Users/{userName}")]
        public User Get(string userName)
        {
            return context.Users.FirstOrDefault(m => m.UserName.ToLower() == userName.ToLower());
        }

        [HttpPost]
        [Route("api/Users")]
        public IHttpActionResult Post([FromBody]User user)
        {
            if (ModelState.IsValid && !IsUserNameUsed(user.UserName))
            {
                context.Users.Add(user);

                context.SaveChanges();

                return Ok();
            }

            return BadRequest();
        }

        [HttpPut]
        [Route("api/Users/{id}")]
        public IHttpActionResult Put(int id, [FromBody]User user)
        {
            if (ModelState.IsValid)
            {
                var userToUpdate = context.Users.FirstOrDefault(m => m.UserId == id);

                if (userToUpdate != null)
                {
                    userToUpdate.Email = user.Email;
                    userToUpdate.Name = user.Name;

                    context.SaveChanges();
                }

                return Ok();
            }

            return BadRequest("");
        }

        [HttpDelete]
        [Route("api/Users/{id}")]
        public IHttpActionResult Delete(int id)
        {
            var userToRemove = context.Users.FirstOrDefault(m => m.UserId == id);

            if (userToRemove != null)
            {
                context.Users.Remove(userToRemove);
                context.SaveChanges();
            }

            return Ok();
        }

        internal bool IsUserNameUsed(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) return false;

            return context.Users.Any(m => m.UserName.ToLower() == userName.ToLower());
        }
    }
}