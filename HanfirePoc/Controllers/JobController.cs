using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace HanfirePoc.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobController : ControllerBase
{
    private readonly IBackgroundJobClient _jobClient;
    private readonly IRecurringJobManager _recurringJobManager;

    public JobController(IBackgroundJobClient jobClient, IRecurringJobManager recurringJobManager)
    {
        _jobClient = jobClient;
        _recurringJobManager = recurringJobManager;
    }

    [HttpGet]
    [Route("/job")]
    public IActionResult Job()
    {
        _jobClient.Enqueue(() => Console.WriteLine("Job Running!"));
        
        return Ok("Job Running. Check hangfire dashboard!");
    }

    [HttpGet]
    [Route("/background-job")]
    public IActionResult BackgroundJob()
    {
        _jobClient.Schedule(() => Console.WriteLine("BackgroundJob Running!"), TimeSpan.FromSeconds(5));
        
        return Ok("Job Running. Check hangfire dashboard!");
    }

    [HttpGet]
    [Route("/recurring-job")]
    public IActionResult RecurringJob()
    {
        // i don't need an endpoint for this one, i just want to see it in the dashboard
        _recurringJobManager.AddOrUpdate("RecurringJob", () => Console.WriteLine("RecurringJob Running!"), Cron.Minutely);
        return Ok("Recurring Job Running. Check hangfire dashboard!");
    }

    [HttpGet]
    [Route("/see")]
    public IActionResult See()
    {
        _jobClient.Schedule<ExampleJob>(x => x.ProcessRevStuff(), TimeSpan.FromMinutes(2));
        return Ok("Check hangfire dashboard!");
    }

}