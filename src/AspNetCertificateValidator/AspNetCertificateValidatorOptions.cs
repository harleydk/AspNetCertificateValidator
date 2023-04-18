using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;

namespace AspNetCertificateValidation;

public class AspNetCertificateValidatorOptions
{
    /// <summary>
    /// Well, it's a logger.
    /// </summary>
    public ILogger Logger { get; set; } = NullLogger.Instance;

    /// <summary>
    /// A <see cref="ICertificateValidator"/>, that may validate a public certificate.
    /// </summary>
    public IEnumerable<ICertificateValidator>? CertificateValidators { get; set; } = new List<ICertificateValidator> { };
}