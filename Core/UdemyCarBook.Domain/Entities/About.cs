using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyCarBook.Domain.Base;

namespace UdemyCarBook.Domain.Entities
{
    public class About : BaseEntity 
    {
        /// <summary>
        /// Hakkımızda başlığı
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Hakkımızda açıklaması
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Hakkımızda resmi URL'si
        /// </summary>
        public string ImageUrl { get; set; }
    }
}
