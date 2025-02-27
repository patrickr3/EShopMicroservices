﻿using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors;

public class LogginBehavior<TRequest, TResponse>
  (ILogger<LogginBehavior<TRequest, TResponse>> logger)
  : IPipelineBehavior<TRequest, TResponse>
  where TRequest : notnull, IRequest<TResponse>
  where TResponse : notnull
{
  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    logger.LogInformation($"[START] Handle request {typeof(TRequest).Name} - Response={typeof(TResponse).Name} - RequestData={request}");

    var timer = new Stopwatch();
    timer.Start();

    var response = await next();

    timer.Stop();
    var timeTaken = timer.Elapsed;
    if (timeTaken.Seconds > 3)
    {
      logger.LogWarning($"[PERFORMANCE] The request {typeof(TRequest).Name} took {timeTaken.Seconds}");
    }

    logger.LogInformation($"[END] Handled {typeof(TRequest).Name} with {typeof(TResponse).Name}");
    return response;
  }
}
