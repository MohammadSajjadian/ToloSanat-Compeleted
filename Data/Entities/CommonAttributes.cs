using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class CommonAttributes
    {
        public int id { get; set; }

        #region String Attributes

        public string title { get; set; }
        public string description { get; set; }

        #endregion

        #region ForeignKeys

        public int languageId { get; set; }
        [ForeignKey(nameof(languageId))]
        public Language language { get; set; }

        #endregion
    }
}
