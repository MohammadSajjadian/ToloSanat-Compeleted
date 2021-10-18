using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Language
    {
        public int id { get; set; }

        public string title { get; set; }

        #region ICollections

        public ICollection<Post> posts { get; set; }
        public ICollection<Project> projects { get; set; }
        public ICollection<ElementProp> elementProps { get; set; }
        public ICollection<SlideBar> slideBars { get; set; }
        public ICollection<Element1> element1s { get; set; }
        public ICollection<Element2> element2s { get; set; }
        public ICollection<Element3> element3s { get; set; }
        public ICollection<Element4> element4s { get; set; }

        #endregion
    }
}
