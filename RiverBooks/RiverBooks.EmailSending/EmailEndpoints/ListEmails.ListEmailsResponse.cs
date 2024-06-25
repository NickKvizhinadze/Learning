namespace RiverBooks.EmailSending.EmailEndpoints;

internal class ListEmailsResponse
{
    public long Count { get; set; }
    public List<EmailOutboxDto> Emails { get; internal set; } = new();
}