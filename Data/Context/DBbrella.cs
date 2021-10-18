using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class DBbrella : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ElementProp> elementProps { get; set; }
        public DbSet<Language> languages { get; set; }
        public DbSet<Post> posts { get; set; }
        public DbSet<Project> projects { get; set; }
        public DbSet<SlideBar> slideBars { get; set; }
        public DbSet<Element1> element1s { get; set; }
        public DbSet<Element2> element2s { get; set; }
        public DbSet<Element3> element3s { get; set; }
        public DbSet<Element4> element4s { get; set; }

        public DBbrella(DbContextOptions<DBbrella> options)
            : base(options)
        {
        }
    }
}
