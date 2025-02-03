using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using UdemyCarBook.Domain.Base;

namespace UdemyCarBook.Domain.Entities
{
    public class Permission : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Group { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; } = true;

        [JsonIgnore]
        public virtual ICollection<Role> Roles { get; set; }

        public Permission()
        {
            Roles = new HashSet<Role>();
        }
    }
} 