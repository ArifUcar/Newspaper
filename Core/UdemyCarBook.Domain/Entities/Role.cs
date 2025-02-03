using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using UdemyCarBook.Domain.Base;

namespace UdemyCarBook.Domain.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;

        [JsonIgnore]
        public virtual ICollection<UserRole> UserRoles { get; set; }

        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }

        [JsonIgnore]
        public virtual User CreatedByUser { get; set; }

        [JsonIgnore]
        public virtual User UpdatedByUser { get; set; }

        [JsonIgnore]
        public virtual User LastModifiedByUser { get; set; }

        [JsonIgnore]
        public virtual ICollection<Permission> Permissions { get; set; }

        public Role()
        {
            Users = new HashSet<User>();
            Permissions = new HashSet<Permission>();
        }
    }
}
