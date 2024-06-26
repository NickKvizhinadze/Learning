namespace RiverBooks.EmailSending.EmailEndpoints;

internal class ListEmailsRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}