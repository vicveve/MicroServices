using Aforo255.Cross.Metric.Registry;
using Aforo255.Cross.Token.Src;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MS.AFORO255.Security.DTOs;
using MS.AFORO255.Security.Services;
using System.Text.Json;

namespace MS.AFORO255.Security.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAccessService _accessService;
    private readonly ILogger<AuthController> _logger;
    private readonly JwtOptions _jwtOption;
    private readonly IMetricsRegistry _metricsRegistry;

    public AuthController(IAccessService accessService, IOptionsSnapshot<JwtOptions> jwtOption, ILogger<AuthController> logger, IMetricsRegistry metricsRegistry)
    {
        _accessService = accessService;
        _logger = logger;
        _jwtOption = jwtOption.Value;
        _metricsRegistry = metricsRegistry;
    }


    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_accessService.GetAll());
    }

    [HttpPost]
    public IActionResult Post([FromBody] AuthRequest request)
    {
        _logger.LogInformation("Post in AuthController with {0}", JsonSerializer.Serialize(request));

        if (!_accessService.Validate(request.UserName, request.Password))
        {
            return Unauthorized();
        }

        string token = JwtToken.Create(_jwtOption);
        Response.Headers.Add("access-control-expose-headers", "Authorization");
        Response.Headers.Add("Authorization", token);
        _metricsRegistry.IncrementFindQuery();
        return Ok(new AuthResponse(token, "5h"));
    }
}

