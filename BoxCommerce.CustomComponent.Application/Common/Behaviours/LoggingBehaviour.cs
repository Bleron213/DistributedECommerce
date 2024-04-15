﻿using BoxCommerce.CustomComponent.Application.Common.Infrastructure;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace BoxCommerce.CustomComponent.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly ICurrentUserService _user;

    public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService user)
    {
        _logger = logger;
        _user = user;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _user.UserId ?? string.Empty;
        string? userName = _user.Username;

        _logger.LogInformation("OrdersAPI MediatR Request: {Name} {@UserId} {@UserName} {@Request}",
            requestName, userId, userName, request);
    }
}
