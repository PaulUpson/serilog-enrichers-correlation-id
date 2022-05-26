using System;
using Serilog.Configuration;
using Serilog.Enrichers;

namespace Serilog
{
    public static class HttpHeaderLoggerConfigurationExtensions
    {
        public static LoggerConfiguration WithHttpHeader(
            this LoggerEnrichmentConfiguration enrichmentConfiguration,
            string headerKey, string propertyName)
        {
            if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
            return enrichmentConfiguration.With(new HttpHeaderEnricher(headerKey, propertyName));
        }
    }
}
