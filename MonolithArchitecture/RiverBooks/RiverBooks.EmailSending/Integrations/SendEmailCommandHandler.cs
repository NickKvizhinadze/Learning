﻿using Ardalis.Result;
using MediatR;
using RiverBooks.EmailSending.Contracts;
using RiverBooks.EmailSending.EmailBackgroundService;

namespace RiverBooks.EmailSending.Integrations;

internal class SendEmailCommandHandler(ISendEmail emailSender) //: IRequestHandler<SendEmailCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        await emailSender.SendEmailAsync(request.To, request.From, request.Subject, request.Body);
        return Result.Success(Guid.Empty);
    }
}