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

        public IEnumerable<User> Get()
        {
            return context.Users.ToList();
        }

        public User Get(string id)
        {
            return context.Users.FirstOrDefault(m => m.UserName.ToLower() == id.ToLower());
        }

        public void Post([FromBody]User user)
        {
            if (IsUserValid(user) && !IsUserNameUsed(user.UserName))
            {
                context.Users.Add(user);

                context.SaveChanges();

            }
        }

        public void Put(string id, [FromBody]User user)
        {
            if (IsUserValid(user))
            {
                var userToUpdate = Get(id);

                userToUpdate.Email = user.Email;
                userToUpdate.Name = user.Name;

                context.SaveChanges();
            }
        }

        public void Delete(string id)
        {
            var userToRemove = context.Users.FirstOrDefault(m => m.UserName.ToLower() == id.ToLower());

            context.Users.Remove(userToRemove);
            context.SaveChanges();

        }

        internal bool IsUserNameUsed(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) return false;

            return context.Users.Any(m => m.UserName.ToLower() == userName.ToLower());
        }

        internal bool IsUserValid(User user)
        {
            // Should also validate email format, username format, but for simplicity's sake..

            if (string.IsNullOrWhiteSpace(user.Name)) return false;
            if (string.IsNullOrWhiteSpace(user.Email)) return false;
            if (string.IsNullOrWhiteSpace(user.UserName)) return false;

            return true;
        }
    }
}