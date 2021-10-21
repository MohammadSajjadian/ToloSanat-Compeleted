using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string  name { get; set; }
        public string  family { get; set; }

        public DateTime tokenCreationTime { get; set; }
    }
}
