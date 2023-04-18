using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AspNetCertificateValidation;

public sealed class CertificateEncryptionValidator : ICertificateValidator, IDisposable
{
    private X509Certificate2? _privateCertificate;
    private readonly string? _pfxPath;
    private readonly string? _certificatePassword;
    private readonly ILogger _logger;
    private bool disposedValue;

    public CertificateEncryptionValidator(string pfxPath, string certificatePassword, ILogger logger)
    {
        _pfxPath = pfxPath;
        _certificatePassword = certificatePassword;
        _logger = logger;
    }

    /// <summary>
    /// Performs a simple encryption and decryption test to verify that the public certificate matches the private certificate.
    /// If the test is successful, it returns true; otherwise, it returns false.
    /// </summary>
    public bool ValidateClientCertificate(X509Certificate2 publicCertificate)
    {
        try
        {
            if (_privateCertificate is null && (string.IsNullOrWhiteSpace(_pfxPath) || string.IsNullOrWhiteSpace(_certificatePassword)))
            {
                throw new ArgumentException("Bad certificate path or password");
            }

            // Initialize private certificate if not already initialized
            _privateCertificate ??= new X509Certificate2(_pfxPath, _certificatePassword);

            string thumbprint = publicCertificate.Thumbprint;
            _logger.LogInformation($"Validating private certificate against public, with thumbprint {thumbprint}.");

            // Get the public key from the client certificate
            RSA publicKey = publicCertificate?.GetRSAPublicKey()
                ?? throw new ArgumentException($"Could not call GetRSAPublicKey() on {nameof(publicCertificate)}.");

            // Get the private key from the self-signed certificate
            RSA privateKey = _privateCertificate.GetRSAPrivateKey()
                ?? throw new ArgumentException($"Could not call GetRSAPrivateKey() on {nameof(_privateCertificate)}.");

            // Use the public key to encrypt a test message
            Guid testMessageGuid = Guid.NewGuid();
            byte[] testData = Encoding.UTF8.GetBytes(testMessageGuid.ToString());
            byte[] encryptedData = publicKey.Encrypt(testData, RSAEncryptionPadding.OaepSHA256);

            // Use the private key to decrypt the encrypted message
            byte[] decryptedData = privateKey.Decrypt(encryptedData, RSAEncryptionPadding.OaepSHA256);

            // If the decrypted message matches the original message, the validation succeeded
            return Encoding.UTF8.GetString(decryptedData) == testMessageGuid.ToString();
        }
        catch (CryptographicException ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing && _privateCertificate is not null)
            {
                _privateCertificate.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}