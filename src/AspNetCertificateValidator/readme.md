



### Eksempel p� konfiguration.

```C#
public void ConfigureServices(IServiceCollection services)
{

    // H�ndt�r modtagelse af certifikater fra klienterne. Uden denne vil vi ikke kunne tilf�je services.AddKpCertificateMiddleware.
    services.AddCertificateForwarding(options =>
    {
        options.CertificateHeader = "KP-API-CERT";
    });

    // H�ndter validering af klient-certifikater mod et private, self-signed certifikat.
    services.AddAspNetCertificateValidator(options =>
    {
        options.Logger = logger;

        List<ICertificateValidator> certificateValidators = new();

        string PfxFilename = @"/Resources/Kp.Stud.AdSync.pfx";
        string PfxPassword = "wallen11";
        certificateValidators.Add(new CertificateEncryptionValidator(PfxFilename, PfxPassword, logger));

        // Eksempel p� tilf�jelse af yderligere validators
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