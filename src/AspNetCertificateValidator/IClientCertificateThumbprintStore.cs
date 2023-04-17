namespace AspNetCertificateValidator;

public interface IClientCertificateThumbprintStore
{
    bool RecognizesThumbprint(string thumbprint);
}