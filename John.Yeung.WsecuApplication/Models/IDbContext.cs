using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace John.Yeung.WsecuApplication.Models
{
    public interface IDbContext
    {
        IDbSet<User> Users { get; }
        int SaveChanges();
    }
}