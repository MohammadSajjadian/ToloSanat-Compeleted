using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public int price { get; set; }

        public string  name { get; set; }
        public string  family { get; set; }

        public DateTime tokenCreationTime { get; set; }
        public DateTime forgotPassTimeSpan { get; set; }

        #region ICollections

        public ICollection<Post> posts { get; set; }
        public ICollection<Project> projects { get; set; }
        public ICollection<Message> messages { get; set; }
        public ICollection<Order> orders { get; set; }

        #endregion
    }
}
