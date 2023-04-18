namespace AspNetCertificateValidation;

public interface IClientCertificateThumbprintStore
{
    bool RecognizesThumbprint(string thumbprint);
}