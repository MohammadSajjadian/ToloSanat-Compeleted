using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Element4
    {
        public int id { get; set; }

        #region String Attributes

        public string preTitle { get; set; }
        public string title { get; set; }

        #endregion

        #region ForeignKeys

        public int languageId { get; set; }
        [ForeignKey(nameof(languageId))]
        public Language language { get; set; }

        #endregion
    }
}
