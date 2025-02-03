using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyCarBook.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace UdemyCarBook.Domain.Entities
{
    public class Comment : BaseEntity
    {
        /// <summary>
        /// Yorum içeriği
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Yorum yapan kullanıcı adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Yorum yapan email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Yorumun onay durumu
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// İlişkili haber
        /// </summary>
        public Guid NewsId { get; set; }
        public virtual News News { get; set; }

        /// <summary>
        /// Üst yoruma bağlı alt yorumlar için
        /// </summary>
        public Guid? ParentCommentId { get; set; }
        public virtual Comment? ParentComment { get; set; }
        public virtual ICollection<Comment> Replies { get; set; }

        /// <summary>
        /// Yorumu oluşturan kullanıcı
        /// </summary>
        [ForeignKey("CreatedById")]
        public virtual User CreatedByUser { get; set; }

        /// <summary>
        /// Son güncelleyen kullanıcı
        /// </summary>
        [ForeignKey("UpdatedById")]
        public virtual User LastModifiedByUser { get; set; }
    }
}
