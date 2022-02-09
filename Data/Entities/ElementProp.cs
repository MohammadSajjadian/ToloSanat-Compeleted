using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class ElementProp
    {
        public int id { get; set; }

        #region Header

        public byte[] mainLogo { get; set; }
        public byte[] secondaryLogo { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string instaLink { get; set; }
        public string telegramLink { get; set; }
        public string siteTopTab { get; set; }
        public string extraDes1 { get; set; }
        public string extraDes2 { get; set; }

        public byte[] projectImage { get; set; }  // 1920 * 630

        #endregion

        #region Element 1

        public string e1PreTitle { get; set; }
        public string e1Title { get; set; }
        public string e1Description { get; set; }

        #endregion

        #region Element 2

        public string e2PreTitle { get; set; }
        public string e2Title { get; set; }
        public string e2Description { get; set; }
        public string e2ImageTitle { get; set; }

        public byte[] e2img { get; set; }

        #endregion

        #region Element 3

        public string e3PreTitle { get; set; }
        public string e3Title { get; set; }
        public string e3Description { get; set; }

        public byte[] e3img { get; set; }

        #endregion

        #region Element 4

        public string e4PreTitle { get; set; }
        public string e4Title { get; set; }

        public byte[] e4img { get; set; }

        #endregion

        #region Element 5

        public string e5Title { get; set; }
        public string e5Description { get; set; }
        public string e5ButtonText { get; set; }
        public string e5ButtonLink { get; set; }

        #endregion

        #region About Us

        public string aboutUsShortDes { get; set; }
        public string aboutUsLongDes { get; set; }

        #endregion

        #region Contact Us

        public string contactUsTitle { get; set; }
        public string contactUsDescription { get; set; }

        #endregion

        #region Order

        public string OrderDescription { get; set; }

        #endregion

        #region ForeignKeys

        public int languageId { get; set; }
        [ForeignKey(nameof(languageId))]
        public Language language { get; set; }

        #endregion
    }
}
