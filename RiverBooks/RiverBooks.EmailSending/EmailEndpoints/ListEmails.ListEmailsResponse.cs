﻿namespace RiverBooks.EmailSending.EmailEndpoints;

internal class ListEmailsResponse
{
    public int Count { get; set; }
    public List<EmailOutboxEntity> Emails { get; internal set; } = new();
}