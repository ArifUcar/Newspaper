using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemyCarBook.Domain.Base
{
    public class BaseHistory
    {
        [Column(TypeName = "char(36)")]
        public Guid Id { get; set; }

        [Column(TypeName = "char(36)")]
        public Guid EntityId { get; set; }

        [Required]
        [StringLength(100)]
        public string EntityName { get; set; }

        [Required]
        public string EntityData { get; set; }

        public DateTime ModifiedDate { get; set; }

        [Required]
        [StringLength(50)]
        public string ModificationType { get; set; }

        [StringLength(100)]
        public string? ModifiedBy { get; set; }
    }
}
