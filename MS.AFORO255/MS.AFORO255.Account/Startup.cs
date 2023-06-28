﻿using Aforo255.Cross.Discovery.Consul;
using Aforo255.Cross.Discovery.Fabio;
using Aforo255.Cross.Metric.Registry;
using Aforo255.Cross.Tracing.Src.Zipkin;
using Microsoft.EntityFrameworkCore;
using MS.AFORO255.Account.Persistences;
using MS.AFORO255.Account.Service;

namespace MS.AFORO255.Account;

public class Startup
{
    public Startup(IConfigurationRoot configuration) => Configuration = configuration;

    public IConfigurationRoot Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddDbContext<ContextDatabase>(
          options =>
          {
              options.UseSqlServer(Configuration["cn:sql"]);
          });

        services.AddScoped<IAccountService, AccountService>();
        services.AddConsul();
        services.AddFabio();
        services.AddJZipkin();
        services.AddTransient<IMetricsRegistry, MetricsRegistry>();
    }

    public void Configure(IApplicationBuilder app, IHostApplicationLifetime lifetime)
    {
        app.UseAuthorization();
    }

    public void ConfigureEndpoints(IEndpointRouteBuilder app, IHostApplicationLifetime lifetime)
    {
        app.MapControllers();
    }
}

