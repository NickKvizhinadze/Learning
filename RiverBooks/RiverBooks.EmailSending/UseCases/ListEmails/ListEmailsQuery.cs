using MediatR;

namespace RiverBooks.EmailSending.UseCases.ListEmails;

internal record ListEmailsQuery(int Page = 1, int PageSize = 10): IRequest<(List<EmailOutboxEntity> emails, long count)>;