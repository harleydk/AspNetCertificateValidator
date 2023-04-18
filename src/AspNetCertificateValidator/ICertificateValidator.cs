using System.Security.Cryptography.X509Certificates;

namespace AspNetCertificateValidation;

public interface ICertificateValidator
{
    bool ValidateClientCertificate(X509Certificate2 publicCertificate);
}


