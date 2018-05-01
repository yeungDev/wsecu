using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace John.Yeung.WsecuApplication.Models
{
    public class TestContext : IDbContext
    {
        public TestContext()
        {
            Users = new TestSet<User>();
        }

        public IDbSet<User> Users { get; set; }

        public int SaveChanges()
        {
            return 0;
        }
    }
}