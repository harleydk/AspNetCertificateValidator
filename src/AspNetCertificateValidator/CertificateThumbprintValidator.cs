using System.Security.Cryptography.X509Certificates;

namespace AspNetCertificateValidation;

public sealed class CertificateThumbprintValidator : ICertificateValidator
{
    private readonly IClientCertificateThumbprintStore _clientCertificateThumbprintStore;

    public CertificateThumbprintValidator(IClientCertificateThumbprintStore clientCertificateThumbprintStore)
    {
        _clientCertificateThumbprintStore = clientCertificateThumbprintStore;
    }

    public bool ValidateClientCertificate(X509Certificate2 publicCertificate)
    {
        string thumbprintFromCertificate = publicCertificate?.Thumbprint
            ?? throw new ArgumentException($"certificate {nameof(publicCertificate)} was null.");

        bool thumbprintRecognized = _clientCertificateThumbprintStore?.RecognizesThumbprint(thumbprintFromCertificate)
            ?? throw new ArgumentException($"{nameof(IClientCertificateThumbprintStore)} null.");

        return thumbprintRecognized;
    }
}


