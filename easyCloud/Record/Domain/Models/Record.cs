namespace easyCloud.Record.Domain.Models;

public class Record
{
    public int UserId { get; set; }
    public int QuoteId { get; set; }
    public int ProviderId { get; set; }
    
    public Quote.Domain.Models.Quote Quote { get; set; }
    public User.Domain.Models.User User { get; set; }
    public Provider.Domain.Models.Provider Provider { get; set; }
}