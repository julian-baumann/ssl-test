using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace SslTest;

public static class Encryption
{
    public static string CertificateName = "127.0.0.1";

    public static X509Certificate2 BuildSelfSignedServerCertificate()
    {
        // var distinguishedName = new X500DistinguishedName($"CN={CertificateName}");
        //
        // using var rsa = RSA.Create(2048);
        // var request = new CertificateRequest(distinguishedName, rsa, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
        //
        // // request.CertificateExtensions.Add(
        // //     new X509KeyUsageExtension(X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature , false));
        // //
        // // request.CertificateExtensions.Add(
        // //     new X509EnhancedKeyUsageExtension(
        // //         new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") }, false));
        //
        // var sanBuilder = new SubjectAlternativeNameBuilder();
        // sanBuilder.AddIpAddress(IPAddress.Loopback);
        // sanBuilder.AddIpAddress(IPAddress.IPv6Loopback);
        // sanBuilder.AddDnsName("localhost");
        // sanBuilder.AddDnsName(Environment.MachineName);
        //
        // request.CertificateExtensions.Add(sanBuilder.Build());
        //
        // var certificate = request.CreateSelfSigned(new DateTimeOffset(DateTime.UtcNow.AddDays(-1)), new DateTimeOffset(DateTime.UtcNow.AddDays(3650)));
        //
        // File.WriteAllBytes("test.xml", certificate.Export(X509ContentType.Pfx, "WeNeedASaf3rPassword"));
        //
        // return new X509Certificate2(certificate.Export(X509ContentType.Pfx, "WeNeedASaf3rPassword"), "WeNeedASaf3rPassword", X509KeyStorageFlags.MachineKeySet);

        var ecdsa = ECDsa.Create();
        var req = new CertificateRequest("CN=localhost", ecdsa, HashAlgorithmName.SHA256);

        var sanBuilder = new SubjectAlternativeNameBuilder();
        sanBuilder.AddIpAddress(IPAddress.Loopback);
        sanBuilder.AddIpAddress(IPAddress.IPv6Loopback);
        sanBuilder.AddDnsName("localhost");
        sanBuilder.AddDnsName("127.0.0.1");
        sanBuilder.AddDnsName(Environment.MachineName);

        req.CertificateExtensions.Add(sanBuilder.Build());
        var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(5));

        // File.WriteAllBytes(SpecialDirectories.Desktop + "/Mycert.pfx", cert.Export(X509ContentType.Pfx, "P@55w0rd"));
        return new X509Certificate2(cert.Export(X509ContentType.Pfx, "P@55w0rd"), "P@55w0rd");
    }
}
