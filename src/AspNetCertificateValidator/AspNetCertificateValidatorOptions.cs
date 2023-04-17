using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;

namespace AspNetCertificateValidator;

public class AspNetCertificateValidatorOptions
{
    /// <summary>
    /// Well, det er en logger.
    /// </summary>
    public ILogger Logger { get; set; } = NullLogger.Instance;

    ///// <summary>
    ///// En <see cref="ICertificateValidator"/>, der kan validere et indkommet public certificate mod et privat certifikat.
    ///// </summary>
    public IEnumerable<ICertificateValidator>? CertificateValidators { get; set; } = new List<ICertificateValidator> { };
}