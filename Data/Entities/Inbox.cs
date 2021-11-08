using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Inbox
    {
        public int id { get; set; }

        #region String Attributes

        public string nameFamily { get; set; }
        public string phonenumber { get; set; }
        public string message { get; set; }

        #endregion

        public bool IsConfirm { get; set; }
    }
}
