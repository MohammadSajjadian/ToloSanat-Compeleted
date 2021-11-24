using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Message
    {
        public int id { get; set; }

        public string text { get; set; }

        public byte[] file { get; set; }

        public bool IsDeleteForAdmin { get; set; }

        public DateTime createTime { get; set; }

        #region ForeignKey

        public string userId { get; set; }
        [ForeignKey(nameof(userId))]
        public ApplicationUser applicationUser { get; set; }

        public int groupId { get; set; }
        [ForeignKey(nameof(groupId))]
        public Group group { get; set; }

        #endregion
    }
}
