



### Eksempel på konfiguration.

```C#
public void ConfigureServices(IServiceCollection services)
{

    // Håndtér modtagelse af certifikater fra klienterne. Uden denne vil vi ikke kunne tilføje services.AddKpCertificateMiddleware.
    services.AddCertificateForwarding(options =>
    {
        options.CertificateHeader = "KP-API-CERT";
    });

    // Håndter validering af klient-certifikater mod et private, self-signed certifikat.
    services.AddAspNetCertificateValidator(options =>
    {
        options.Logger = logger;

        List<ICertificateValidator> certificateValidators = new();

        string PfxFilename = @"/Resources/Kp.Stud.AdSync.pfx";
        string PfxPassword = "wallen11";
        certificateValidators.Add(new CertificateEncryptionValidator(PfxFilename, PfxPassword, logger));

        // Eksempel på tilføjelse af yderligere validators
        //string acceptableClientCertificateThumbprint = @"399C573E330794FFABB015558DA0A2095FF5C0EC";
        //IClientCertificateThumbprintStore clientCertificateThumbprintStore = new FakeClientCertificateThumbprintStoreForTest(acceptableClientCertificateThumbprint);
        //certificateValidators.Add(new CertificateThumbprintValidator(clientCertificateThumbprintStore));

        options.CertificateValidators = certificateValidators;
    });
}

public void Configure(WebApplication app)
{
    _ = app.UseCertificateForwarding();
    _ = app.UseMiddleware<AspNetCertificateValidator>();
}
```