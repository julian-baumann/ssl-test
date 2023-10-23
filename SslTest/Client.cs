using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace SslTest;

public class Client
{
    private readonly TcpClient _client = new();

    private static bool ValidateServerCertificate(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
    {
        if (sslPolicyErrors == SslPolicyErrors.None)
        {
            return true;
        }

        Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

        // Do not allow this client to communicate with unauthenticated servers.
        return false;
    }

    public async Task SendMessage()
    {
        await _client.ConnectAsync("localhost", 43034);
        await using var sslStream = new SslStream(
            _client.GetStream(),
            false,
            new RemoteCertificateValidationCallback(ValidateServerCertificate),
            null
        );

        await sslStream.AuthenticateAsClientAsync("localhost");

        await using var streamWriter = new StreamWriter(sslStream);
        await streamWriter.WriteLineAsync(Messages.HelloWorld);
    }
}
