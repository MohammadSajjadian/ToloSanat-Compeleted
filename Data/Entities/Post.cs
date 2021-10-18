using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Post
    {
        public int id { get; set; }

        #region String Attributes

        public string title { get; set; }
        public string shortDecpription { get; set; }
        public string longDecpription { get; set; }

        #endregion

        public DateTime createTime { get; set; } = DateTime.Now;

        #region ForeignKeys

        public string userId { get; set; }
        [ForeignKey(nameof(userId))]
        public ApplicationUser ApplicationUser { get; set; }

        public int languageId { get; set; }
        [ForeignKey(nameof(languageId))]
        public Language language { get; set; }

        #endregion
    }
}
