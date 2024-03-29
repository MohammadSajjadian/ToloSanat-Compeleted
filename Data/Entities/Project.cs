﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class Project
    {
        public int id { get; set; }

        #region String Attributes

        public string title { get; set; }
        public string Description { get; set; }

        #endregion

        #region Image Attributes

        public byte[] thumbNail { get; set; } // 270 * 300
        public byte[] detailImg { get; set; } // 770 * 470

        #endregion

        #region Bool Attributes

        public bool IsSms { get; set; }
        public bool IsEmail { get; set; }

        #endregion

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
