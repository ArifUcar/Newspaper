using System;
using System.Collections.Generic;

namespace UdemyCarBook.Application.Features.Mediator.Results.NewsResults
{
    public class GetNewsQueryResult
    {
        /// <summary>
        /// Haber ID'si
        /// </summary>
        public Guid Id { get; set; }

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
        public string CoverImageUrl { get; set; }

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
        public DateTime? PublishDate { get; set; }

        /// <summary>
        /// Oluşturulma tarihi
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Son güncelleme tarihi
        /// </summary>
        public DateTime? LastModifiedDate { get; set; }

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
        /// Kategori adı
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Kullanıcı ID'si
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Kullanıcı adı
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Kullanıcı resmi
        /// </summary>
        public string UserImageUrl { get; set; }

        /// <summary>
        /// Haber durumu
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Yorum sayısı
        /// </summary>
        public int CommentCount { get; set; }

        /// <summary>
        /// Etiketler
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// Oluşturan kullanıcı adı
        /// </summary>
        public string CreatedByUserName { get; set; }

        /// <summary>
        /// Son güncelleyen kullanıcı adı
        /// </summary>
        public string? LastModifiedByUserName { get; set; }
    }
} 