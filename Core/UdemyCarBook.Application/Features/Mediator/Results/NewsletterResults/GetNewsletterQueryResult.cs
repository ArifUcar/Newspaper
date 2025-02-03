namespace UdemyCarBook.Application.Features.Mediator.Results.NewsletterResults
{
    public class GetNewsletterQueryResult
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime SubscriptionDate { get; set; }
        public DateTime? UnsubscribeDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByUserName { get; set; }
    }
} 