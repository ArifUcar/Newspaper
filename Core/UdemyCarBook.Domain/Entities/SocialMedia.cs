using System;
using System.ComponentModel.DataAnnotations.Schema;
using UdemyCarBook.Domain.Base;

namespace UdemyCarBook.Domain.Entities
{
    public class SocialMedia : BaseEntity
    {
        /// <summary>
        /// Platform adı (Instagram, Facebook, Twitter vb.)
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// Sosyal medya URL'i
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Platform ikonu (Font Awesome veya başka ikon kütüphanesi için)
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Sıralama için kullanılacak değer
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Aktif/Pasif durumu
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Sosyal medya hesabının takipçi sayısı
        /// </summary>
        public int? FollowerCount { get; set; }

        /// <summary>
        /// Sosyal medya hesap adı/kullanıcı adı
        /// </summary>
        public string? AccountName { get; set; }

        /// <summary>
        /// İlişkili olduğu yazar (opsiyonel)
        /// </summary>
        public Guid? UserId { get; set; }
        public virtual User? User { get; set; }

        [ForeignKey("CreatedById")]
        public virtual User CreatedByUser { get; set; }

        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }

        [ForeignKey("LastModifiedByUserId")]
        public virtual User LastModifiedByUser { get; set; }
    }
}
