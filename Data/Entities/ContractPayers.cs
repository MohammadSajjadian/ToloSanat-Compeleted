using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class ContractPayers
    {
        public int id { get; set; }

        public string userId { get; set; }

        public int trackingCode { get; set; }

        public DateTime payTime { get; set; }

        public int price { get; set; }

        public bool IsAdminConfirmed { get; set; }
    }
}
