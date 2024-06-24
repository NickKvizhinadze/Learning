using RiverBooks.EmailSending.Integrations;

namespace RiverBooks.EmailSending.Interfaces;

internal interface IOutboxService
{
    Task QueueEmailForSending(EmailOutboxEntity entity);
}