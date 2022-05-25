using System;
using Serilog.Configuration;
using Serilog.Enrichers;

namespace Serilog
{
    public static class CustomHeaderLoggerConfigurationExtensions
    {
        public static LoggerConfiguration WithCustomHeader(
            this LoggerEnrichmentConfiguration enrichmentConfiguration,
            string headerKey, string propertyName)
        {
            if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
            return enrichmentConfiguration.With(new CustomHeaderEnricher(headerKey, propertyName));
        }
    }
}
