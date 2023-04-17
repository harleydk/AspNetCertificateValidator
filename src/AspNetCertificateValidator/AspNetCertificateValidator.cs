using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace AspNetCertificateValidator;

public class AspNetCertificateValidator
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly IEnumerable<ICertificateValidator>? _certificateValidators;

    public AspNetCertificateValidator(RequestDelegate next, IOptions<AspNetCertificateValidatorOptions> options)
    {
        _certificateValidators = options.Value.CertificateValidators
            ?? throw new ArgumentException($"{nameof(_certificateValidators)} null.");

        _logger = options.Value.Logger 
            ?? throw new ArgumentException($"{nameof(_logger)} null.");

        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Retrieve the client certificate from the connection
            X509Certificate2 clientCertificate = context.Connection.ClientCertificate
                ?? throw new ArgumentException($"{nameof(AspNetCertificateValidator)}-{nameof(InvokeAsync)}: no valid certificate.");

            bool clientCertificateValidates = _certificateValidators?.All(validator =>
                validator.ValidateClientCertificate(clientCertificate))
                ?? throw new ArgumentException($"{nameof(_certificateValidators)} is null.");

            if (!clientCertificateValidates)
            {
                string errormessage = $"Bad public certificate - request will be aborted.";
                _logger.LogError(new ArgumentException(errormessage), errormessage);
                context.Abort();
            }

            // Set the result of the validation in the HttpContext.Items collection
            context.Items["ClientCertificateValid"] = true;

            // Call the next middleware in the pipeline
            await _next(context);
        }
        catch (ArgumentException ex) when (ex.Message.Contains("no certificate was sent along"))
        {
            _logger.LogError(ex, @$"No certificate was sent along from the client. 
                Confirm you've got a 'app.UseCertificateForwarding()' and 
                a 'app.UseMiddleware<{nameof(AspNetCertificateValidator)}>() has been added BEFORE that line.");
            throw;
        }
    }
}