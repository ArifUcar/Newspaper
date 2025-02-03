using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using UdemyCarBook.Domain.Base;

namespace UdemyCarBook.Domain.Entities
{
    public class Tag : BaseEntity
    {
        /// <summary>
        /// Etiket adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Etiketin bağlı olduğu haberler
        /// </summary>
        public virtual ICollection<News> News { get; set; }

        [ForeignKey("CreatedById")]
        public virtual User CreatedByUser { get; set; }

        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }

        [ForeignKey("LastModifiedByUserId")]
        public virtual User LastModifiedByUser { get; set; }

        public Tag()
        {
            News = new HashSet<News>();
        }
    }
}
