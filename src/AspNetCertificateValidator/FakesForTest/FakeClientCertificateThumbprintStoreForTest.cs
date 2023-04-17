namespace AspNetCertificateValidator.FakesForTest;

public sealed class FakeClientCertificateThumbprintStoreForTest : IClientCertificateThumbprintStore
{
    private readonly string _fakeThumbprintToReturn;

    public FakeClientCertificateThumbprintStoreForTest(string fakeThumbprintToReturn)
    {
        _fakeThumbprintToReturn = fakeThumbprintToReturn;
    }

    public bool RecognizesThumbprint(string thumbprint)
    {
        return _fakeThumbprintToReturn.Equals(thumbprint, StringComparison.OrdinalIgnoreCase);
    }
}