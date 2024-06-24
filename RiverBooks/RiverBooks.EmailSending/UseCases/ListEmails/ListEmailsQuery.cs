using MediatR;

namespace RiverBooks.EmailSending.UseCases.ListEmails;

internal record ListEmailsQuery: IRequest<List<EmailOutboxEntity>>;