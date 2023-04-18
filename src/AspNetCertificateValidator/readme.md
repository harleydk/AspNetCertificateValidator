



### How to set it up.

```C#

builder.Services.AddCertificateForwarding(options =>
{
    // Add this header to the headers of the client's request. Value is the client's certificate, as a Base64-encoded string.
    options.CertificateHeader = "X-ARR-ClientCert";
});

builder.Services.AddAspNetCertificateValidator(options =>
{
    options.Logger = logger; // Replace with your own.

    List<ICertificateValidator> certificateValidators = new(); // Instantiate your own validators.

    string PfxFilename = config["PrivateCertPath"]
        ?? throw new ArgumentException($"No PrivateCertPath in config.");
    string PfxPassword = config["PrivateCertPassword"]
       ?? throw new ArgumentException($"No PrivateCertPassword in config.");
    certificateValidators.Add(new CertificateEncryptionValidator(PfxFilename, PfxPassword, logger));

    string acceptableClientCertificateThumbprint = config["ClientPublicThumbprint"]
        ?? throw new ArgumentException($"No ClientPublicThumbprint in config.");

    IClientCertificateThumbprintStore clientCertificateThumbprintStore = new FakeClientCertificateThumbprintStoreForTest(acceptableClientCertificateThumbprint);
    certificateValidators.Add(new CertificateThumbprintValidator(clientCertificateThumbprintStore));

    options.CertificateValidators = certificateValidators;
});

var app = builder.Build();

_ = app.UseCertificateForwarding();
_ = app.UseMiddleware<AspNetCertificateValidator>();
_ = app.UseHttpsRedirection();

_ = app.UseAuthentication();
_ = app.UseAuthorization();
```