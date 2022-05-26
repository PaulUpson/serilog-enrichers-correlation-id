# Serilog.Enrichers.HttpHeader

Enriches Serilog events with a custom http header for tracking requests.

<!-- [![Build status](https://ci.appveyor.com/api/projects/status/c280e547sj758qfc/branch/master?svg=true)](https://ci.appveyor.com/project/ejcoyle88/serilog-enrichers-http-header/branch/master) -->
<!-- [![Coverage Status](https://coveralls.io/repos/github/ekmsystems/serilog-enrichers-correlation-id/badge.svg?branch=master)](https://coveralls.io/github/ekmsystems/serilog-enrichers-http-header?branch=master) -->
[![NuGet](http://img.shields.io/nuget/v/Serilog.Enrichers.HttpHeader.svg?style=flat)](https://www.nuget.org/packages/Serilog.Enrichers.HttpHeader/)

To use the enricher, first install the NuGet package:

```powershell
Install-Package Serilog.Enrichers.HttpHeader
```

Then, apply the enricher to your `LoggerConfiguration`:

```csharp
Log.Logger = new LoggerConfiguration()
    .Enrich.WithHttpHeader("x-correlation-id", "CorrelationId")
    // ...other configuration...
    .CreateLogger();
```

The `WithHttpHeader(headerKey, propertyName)` enricher allows you to specify a headerKey to retrieve from the http request and the name of the property to use when logging.

## Installing into an ASP.NET Core Web Application

This is what your `Startup` class should contain in order for this enricher to work as expected:

```cs
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace MyWebApp
{
    public class Startup
    {
        public Startup()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {CorrelationId} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .Enrich.WithHttpHeader("x-correlation-id", "CorrelationId")
                .CreateLogger();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // ...
            services.AddHttpContextAccessor();
            // ...
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // ...
            loggerFactory.AddSerilog();
            // ...
        }
    }
}
```

You need to register the `IHttpContextAccessor` singleton so that the enricher has access to the requests `HttpContext`.
