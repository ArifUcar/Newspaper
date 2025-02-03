using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyCarBook.Domain.Base;

namespace UdemyCarBook.Domain.Entities
{
    public class Category : BaseEntity
    {
        /// <summary>
        /// Kategori adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Kategori açıklaması
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Kategori ikonu/görseli
        /// </summary>
        public string? IconUrl { get; set; }

        /// <summary>
        /// Bu kategorideki haberler
        /// </summary>
        public virtual ICollection<News> News { get; set; }

        [ForeignKey("CreatedById")]
        public virtual User CreatedByUser { get; set; }

        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }

        [ForeignKey("LastModifiedByUserId")]
        public virtual User LastModifiedByUser { get; set; }

        public Category()
        {
            News = new HashSet<News>();
        }
    }
}
