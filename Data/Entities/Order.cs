using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Order
    {
        public int id { get; set; }

        public int trackingCode { get; set; }

        public DateTime payTime { get; set; }

        public int price { get; set; }

        public bool IsAdminConfirmed { get; set; }

        #region ForeignKey

        public string userId { get; set; }
        [ForeignKey(nameof(userId))]
        public ApplicationUser applicationUser { get; set; }

        #endregion
    }
}
