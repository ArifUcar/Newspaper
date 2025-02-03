using System;
using System.ComponentModel.DataAnnotations;

namespace UdemyCarBook.Domain.Base
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Guid? UpdatedByUserId { get; set; }
        public Guid? LastModifiedByUserId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
