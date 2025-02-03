namespace UdemyCarBook.Application.Features.Mediator.Results.NewsletterResults
{
    public class GetNewsletterByIdQueryResult
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime SubscriptionDate { get; set; }
        public DateTime? UnsubscribeDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByUserName { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string LastModifiedByUserName { get; set; }
    }
} 