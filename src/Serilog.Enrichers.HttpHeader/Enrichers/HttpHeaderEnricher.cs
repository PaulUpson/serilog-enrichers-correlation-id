using System;
using System.Linq;
using Serilog.Core;
using Serilog.Events;

#if NETFULL
using Serilog.Enrichers.HttpHeader.Accessors;
using Serilog.Enrichers.HttpHeader.Extensions;
#else
using Microsoft.AspNetCore.Http;
#endif

namespace Serilog.Enrichers
{
    public class HttpHeaderEnricher : ILogEventEnricher
    {
        private readonly string _propertyName;
        private readonly string _headerKey;
        private readonly IHttpContextAccessor _contextAccessor;

        public HttpHeaderEnricher(string headerKey, string propertyName) : this(headerKey, propertyName, new HttpContextAccessor())
        {
        }

        internal HttpHeaderEnricher(string headerKey, string propertyName, IHttpContextAccessor contextAccessor)
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
