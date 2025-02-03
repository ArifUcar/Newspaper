using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using UdemyCarBook.Domain.Base;
using UdemyCarBook.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace UdemyCarBook.Domain.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        [JsonIgnore]
        public virtual ICollection<News> News { get; set; }
        public string? Description { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public UserType UserType { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpireDate { get; set; }
      
        /// <summary>
        /// Kapak resmi URL'si
        /// </summary>
        public string CoverImageUrl { get; set; }


        public string CoverImageFileName { get; set; }

        /// <summary>
        /// Kapak resmi MIME tipi
        /// </summary>
        public string CoverImageContentType { get; set; }

        /// <summary>
        /// Kapak resmi boyutu (byte)
        /// </summary>
        public long? CoverImageSize { get; set; }

        /// <summary>
        /// Kapak resmi genişliği (px)
        /// </summary>
        public int? CoverImageWidth { get; set; }

        /// <summary>
        /// Kapak resmi yüksekliği (px)
        /// </summary>
        public int? CoverImageHeight { get; set; }

        /// <summary>
        /// Kapak resmi yükleme tarihi
        /// </summary>
        public DateTime? CoverImageUploadDate { get; set; }

        [JsonIgnore]
        public virtual ICollection<UserRole> UserRoles { get; set; }

        [JsonIgnore]
        public virtual ICollection<Role> Roles { get; set; }

        [JsonIgnore]
        public virtual ICollection<Role> CreatedRoles { get; set; }

        [JsonIgnore]
        public virtual ICollection<Role> UpdatedRoles { get; set; }

        [JsonIgnore]
        public virtual ICollection<Role> LastModifiedRoles { get; set; }

        [JsonIgnore]
        public virtual ICollection<User> CreatedByUsers { get; set; }

        [JsonIgnore]
        public virtual ICollection<User> UpdatedByUsers { get; set; }

        [JsonIgnore]
        public virtual ICollection<User> LastModifiedByUsers { get; set; }

        [JsonIgnore]
        public virtual User? CreatedByUser { get; set; }

        [JsonIgnore]
        public virtual User? UpdatedByUser { get; set; }

        [JsonIgnore]
        public virtual User? LastModifiedByUser { get; set; }

        [JsonIgnore]
        public virtual ICollection<News> CreatedNews { get; set; }

        [JsonIgnore]
        public virtual ICollection<News> UpdatedNews { get; set; }

        [JsonIgnore]
        public virtual ICollection<News> LastModifiedNews { get; set; }

        [JsonIgnore]
        public virtual ICollection<Comment> CreatedComments { get; set; }

        [JsonIgnore]
        public virtual ICollection<Comment> UpdatedComments { get; set; }

        [JsonIgnore]
        public virtual ICollection<Comment> LastModifiedComments { get; set; }

        [JsonIgnore]
        public virtual ICollection<Category> CreatedCategories { get; set; }

        [JsonIgnore]
        public virtual ICollection<Category> UpdatedCategories { get; set; }

        [JsonIgnore]
        public virtual ICollection<Category> LastModifiedCategories { get; set; }

        [JsonIgnore]
        public virtual ICollection<Tag> CreatedTags { get; set; }

        [JsonIgnore]
        public virtual ICollection<Tag> UpdatedTags { get; set; }

        [JsonIgnore]
        public virtual ICollection<Tag> LastModifiedTags { get; set; }

        [JsonIgnore]
        public virtual ICollection<SocialMedia> CreatedSocialMedias { get; set; }

        [JsonIgnore]
        public virtual ICollection<SocialMedia> UpdatedSocialMedias { get; set; }

        [JsonIgnore]
        public virtual ICollection<SocialMedia> LastModifiedSocialMedias { get; set; }

        [JsonIgnore]
        public virtual ICollection<SocialMedia> SocialMedias { get; set; }

        [JsonIgnore]
        public virtual ICollection<Contact> CreatedContacts { get; set; }

        [JsonIgnore]
        public virtual ICollection<Contact> UpdatedContacts { get; set; }

        [JsonIgnore]
        public virtual ICollection<Contact> LastModifiedContacts { get; set; }

        [JsonIgnore]
        public virtual ICollection<Newsletter> CreatedNewsletters { get; set; }

        [JsonIgnore]
        public virtual ICollection<Newsletter> UpdatedNewsletters { get; set; }

        [JsonIgnore]
        public virtual ICollection<Newsletter> LastModifiedNewsletters { get; set; }

        [NotMapped]
        [JsonIgnore]
        public virtual ICollection<BaseEntity> CreatedRecords { get; set; }

        [NotMapped]
        [JsonIgnore]
        public virtual ICollection<BaseEntity> UpdatedRecords { get; set; }

        public User()
        {
            News = new HashSet<News>();
            Roles = new HashSet<Role>();
            UserRoles = new HashSet<UserRole>();
            CreatedContacts = new HashSet<Contact>();
            UpdatedContacts = new HashSet<Contact>();
            LastModifiedContacts = new HashSet<Contact>();
            CreatedCategories = new HashSet<Category>();
            UpdatedCategories = new HashSet<Category>();
            LastModifiedCategories = new HashSet<Category>();
            CreatedComments = new HashSet<Comment>();
            UpdatedComments = new HashSet<Comment>();
            LastModifiedComments = new HashSet<Comment>();
            CreatedNewsletters = new HashSet<Newsletter>();
            UpdatedNewsletters = new HashSet<Newsletter>();
            LastModifiedNewsletters = new HashSet<Newsletter>();
            CreatedSocialMedias = new HashSet<SocialMedia>();
            UpdatedSocialMedias = new HashSet<SocialMedia>();
            LastModifiedSocialMedias = new HashSet<SocialMedia>();
            SocialMedias = new HashSet<SocialMedia>();
            CreatedTags = new HashSet<Tag>();
            UpdatedTags = new HashSet<Tag>();
            LastModifiedTags = new HashSet<Tag>();
            CreatedRoles = new HashSet<Role>();
            UpdatedRoles = new HashSet<Role>();
            LastModifiedRoles = new HashSet<Role>();
            CreatedByUsers = new HashSet<User>();
            UpdatedByUsers = new HashSet<User>();
            LastModifiedByUsers = new HashSet<User>();
            CreatedNews = new HashSet<News>();
            UpdatedNews = new HashSet<News>();
            LastModifiedNews = new HashSet<News>();
        }
        public void UpdateImageInfo(string fileName, string contentType, long size, int width, int height)
        {
            CoverImageFileName = fileName;
            CoverImageContentType = contentType;
            CoverImageSize = size;
            CoverImageWidth = width;
            CoverImageHeight = height;
            CoverImageUploadDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Resim bilgilerini temizler
        /// </summary>
        public void ClearImageInfo()
        {
            CoverImageUrl = null;
            CoverImageFileName = null;
            CoverImageContentType = null;
            CoverImageSize = null;
            CoverImageWidth = null;
            CoverImageHeight = null;
            CoverImageUploadDate = null;
        }
    

    public string FullName => $"{FirstName} {LastName}";
    }
}
