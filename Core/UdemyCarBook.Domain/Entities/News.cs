using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using UdemyCarBook.Domain.Base;


namespace UdemyCarBook.Domain.Entities
{
    public class News : BaseEntity
    {
        /// <summary>
        /// Haber başlığı
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Haber içeriği
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Haber özeti
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// SEO dostu URL
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// Kapak resmi URL'si
        /// </summary>
        public string? CoverImageUrl { get; set; }

        /// <summary>
        /// Kapak resmi dosya adı
        /// </summary>
        public string? CoverImageFileName { get; set; }

        /// <summary>
        /// Kapak resmi MIME tipi
        /// </summary>
        public string? CoverImageContentType { get; set; }

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

        /// <summary>
        /// Görüntülenme sayısı
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// Öne çıkarılma durumu
        /// </summary>
        public bool IsFeatured { get; set; }

        /// <summary>
        /// Aktif/Pasif durumu
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Yayınlanma durumu
        /// </summary>
        public bool IsPublished { get; set; }

        /// <summary>
        /// Yayınlanma tarihi
        /// </summary>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// Meta başlık (SEO için)
        /// </summary>
        public string? MetaTitle { get; set; }

        /// <summary>
        /// Meta açıklama (SEO için)
        /// </summary>
        public string? MetaDescription { get; set; }

        /// <summary>
        /// Meta anahtar kelimeler (SEO için)
        /// </summary>
        public string? MetaKeywords { get; set; }

        /// <summary>
        /// Kategori ID'si
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Yazar ID'si
        /// </summary>
        public Guid UserId { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }

        /// <summary>
        /// Haber durumu
        /// </summary>
        public NewsStatus Status { get; set; }

        // Navigation Properties
        public virtual Category Category { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }

        [ForeignKey("CreatedById")]
        public virtual User CreatedByUser { get; set; }

        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }

        [ForeignKey("LastModifiedByUserId")]
        public virtual User LastModifiedByUser { get; set; }

        public News()
        {
            Comments = new HashSet<Comment>();
            Tags = new HashSet<Tag>();
            ViewCount = 0;
            IsActive = true;
            Status = NewsStatus.Draft;
            CreatedDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Resim bilgilerini günceller
        /// </summary>
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
    }

    public enum NewsStatus
    {
        Draft = 0,
        Published = 1,
        Archived = 2
    }
}
