using DemoProjectBOL.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoProjectDAL
{
    public class DemoProjectContext : IdentityDbContext<ApplicationUser>
    {
        public DemoProjectContext()
            : base("DemoProjectContext", throwIfV1Schema: false)
        {
        }
        public DbSet<DevTest> DevTest { get; set; }

        public static DemoProjectContext Create()
        {
            return new DemoProjectContext();
        }
    }
}
