using Aforo255.Cross.Metric.Registry;
using Microsoft.AspNetCore.Mvc;
using MS.AFORO255.Account.Service;

namespace MS.AFORO255.Account.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IMetricsRegistry _metricsRegistry;
    private readonly ILogger<AccountController> _logger;
    public AccountController(IAccountService accountService, IMetricsRegistry metricsRegistry, ILogger<AccountController> logger)
    {
        _accountService = accountService;
        _metricsRegistry = metricsRegistry;
        _logger = logger;
    }


    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation("GET in AccountController with {0}", "");
        _metricsRegistry.IncrementFindQuery();
        return Ok(_accountService.GetAll());
    }
}

