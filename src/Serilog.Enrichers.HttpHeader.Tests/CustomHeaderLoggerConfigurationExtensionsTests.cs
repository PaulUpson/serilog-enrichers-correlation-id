using System;
using NUnit.Framework;
using Serilog.Configuration;
using Serilog.Tests.Support;

namespace Serilog.Tests
{
    [TestFixture]
    [Parallelizable]
    public class HttpHeaderLoggerConfigurationExtensionsTests
    {
        [Test]
        public void WithCorrelationIdHeader_ThenLoggerIsCalled_ShouldNotThrowException()
        {
            var logger = new LoggerConfiguration()
                .Enrich.WithHttpHeader("x-correlation-id", "CorrelationId")
                .WriteTo.Sink(new DelegateSink.DelegatingSink(e => { }))
                .CreateLogger();

            Assert.DoesNotThrow(() => logger.Information("LOG"));
        }

        [Test]
        public void WithCorrelationIdHeader_WhenLoggerEnrichmentConfigurationIsNull_ShouldThrowArgumentNullException()
        {
            LoggerEnrichmentConfiguration configuration = null;
            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentNullException>(() => configuration.WithHttpHeader("x-correlation-id", "CorrelationId"));
        }
    }
}
