using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Group
    {
        public int id { get; set; }

        public string userId { get; set; }

        public bool IsDeleteForAdmin { get; set; }

        public DateTime lastMessageTime { get; set; }

        public ICollection<Message> messages { get; set; }
    }
}
