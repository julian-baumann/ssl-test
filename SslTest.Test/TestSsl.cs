using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic.FileIO;
using NUnit.Framework;

namespace SslTest.Test;

public class TestSsl
{
    private readonly Server _server = new();
    private readonly Client _client = new();

    [Test]
    public async Task GenerateCertificate()
    {
        var ecdsa = ECDsa.Create(); // generate asymmetric key pair
        var req = new CertificateRequest("cn=foobar", ecdsa, HashAlgorithmName.SHA256);
        var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(5));

        // Create PFX (PKCS #12) with private key
        File.WriteAllBytes(SpecialDirectories.Desktop + "/Mycert.pfx", cert.Export(X509ContentType.Pfx, "P@55w0rd"));

        // Create Base 64 encoded CER (public key only)
        File.WriteAllText(SpecialDirectories.Desktop + "/Mycert.cer",
            "-----BEGIN CERTIFICATE-----\r\n"
            + Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks)
            + "\r\n-----END CERTIFICATE-----");
    }

    [Test]
    public async Task Test1()
    {
        Task.Run(async () =>
        {
            await _client.SendMessage();
        });

        var result = await _server.MessageReceived();

        Assert.That(result, Is.True);
    }
}
