using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Province
    {
        public int id { get; set; }

        public string name { get; set; }

        public int price { get; set; }

        public ICollection<TransportationPayers> transportationPayers { get; set; }
    }
}
