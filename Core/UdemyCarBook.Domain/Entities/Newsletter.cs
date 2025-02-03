using System;
using System.ComponentModel.DataAnnotations.Schema;
using UdemyCarBook.Domain.Base;

namespace UdemyCarBook.Domain.Entities
{
    public class Newsletter : BaseEntity
    {
        /// <summary>
        /// Abone email adresi
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Abonelik durumu
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Abonelik doğrulama durumu
        /// </summary>
        public bool IsVerified { get; set; }

        /// <summary>
        /// Abonelik doğrulama tokeni
        /// </summary>
        public string VerificationToken { get; set; }

        /// <summary>
        /// Abonelik tarihi
        /// </summary>
        public DateTime SubscriptionDate { get; set; }

        /// <summary>
        /// Abonelik doğrulama tarihi
        /// </summary>
        public DateTime? VerificationDate { get; set; }

        /// <summary>
        /// Abonelik iptal tarihi
        /// </summary>
        public DateTime? UnsubscribeDate { get; set; }

        /// <summary>
        /// Abonelik oluşturan kullanıcı
        /// </summary>
        [ForeignKey("CreatedById")]
        public virtual User CreatedByUser { get; set; }

        /// <summary>
        /// Abonelik güncelleyen kullanıcı
        /// </summary>
        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }

        /// <summary>
        /// Abonelik son değiştiren kullanıcı
        /// </summary>
        [ForeignKey("LastModifiedByUserId")]
        public virtual User LastModifiedByUser { get; set; }
    }
}
