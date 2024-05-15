using Hangfire.Server;

namespace HanfirePoc;

public class ExampleJob
{
    private readonly ILogger<ExampleJob> _logger;

    public ExampleJob(ILogger<ExampleJob> logger)
    {
        _logger = logger;
    }

    public void ProcessRevStuff()
    {
        _logger.LogInformation("ProcessRevStuff has been triggered");
    }
}