using Microsoft.Extensions.DependencyInjection;

namespace AspNetCertificateValidation;

public static class AspNetCertificateValidatorExtentions
{
    /// <summary>
    /// Add <see cref="AspNetCertificateValidatorOptions"/> to the <see cref="IServiceCollection" />,
    /// used from 'app.AddMiddleware<<see cref="AspNetCertificateValidator"/>>.
    /// </summary>
    public static IServiceCollection AddAspNetCertificateValidator(
        this IServiceCollection services,
        Action<AspNetCertificateValidatorOptions> configure)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        _ = services.AddOptions<AspNetCertificateValidatorOptions>()
            .Validate(o => o.CertificateValidators is not null && o.CertificateValidators.Any(), $"{nameof(AspNetCertificateValidatorOptions.CertificateValidators)} cannot be null or empty.");
        return services.Configure(configure);
    }
}