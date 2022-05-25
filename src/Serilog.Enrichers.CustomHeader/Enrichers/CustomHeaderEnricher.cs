using System;
using System.Linq;
using Serilog.Core;
using Serilog.Events;

#if NETFULL
using Serilog.Enrichers.CustomHeader.Accessors;
using Serilog.Enrichers.CustomHeader.Extensions;
#else
using Microsoft.AspNetCore.Http;
#endif

namespace Serilog.Enrichers
{
    public class CustomHeaderEnricher : ILogEventEnricher
    {
        private readonly string _propertyName;
        private readonly string _headerKey;
        private readonly IHttpContextAccessor _contextAccessor;

        public CustomHeaderEnricher(string headerKey, string propertyName) : this(headerKey, propertyName, new HttpContextAccessor())
        {
        }

        internal CustomHeaderEnricher(string headerKey, string propertyName, IHttpContextAccessor contextAccessor)
        {
            _headerKey = headerKey;
            _propertyName = propertyName;
            _contextAccessor = contextAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (_contextAccessor.HttpContext == null)
                return;

            var headerValue = GetHeaderValue();

            var headerValueProperty = new LogEventProperty(_propertyName, new ScalarValue(headerValue));

            logEvent.AddOrUpdateProperty(headerValueProperty);
        }

        private string GetHeaderValue()
        {
            var header = string.Empty;

            if (_contextAccessor.HttpContext.Request.Headers.TryGetValue(_headerKey, out var values))
            {
                header = values.FirstOrDefault();
            }
            else if (_contextAccessor.HttpContext.Response.Headers.TryGetValue(_headerKey, out values))
            {
                header = values.FirstOrDefault();
            }

            return header;
        }
    }
}
