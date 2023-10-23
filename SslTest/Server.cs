using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace SslTest;

public class Server
{
    private readonly TcpListener _tcpListener;

    public Server()
    {
        _tcpListener = new TcpListener(IPAddress.Any, 43034);
        _tcpListener.Start();
    }

    public async Task<bool> MessageReceived()
    {
        var connection = await _tcpListener.AcceptTcpClientAsync();

        await using var sslStream = new SslStream(connection.GetStream(), false);
        await sslStream.AuthenticateAsServerAsync(Encryption.BuildSelfSignedServerCertificate(), clientCertificateRequired: false, checkCertificateRevocation: true);

        using var streamReader = new StreamReader(sslStream);
        var message = await streamReader.ReadLineAsync();

        return message == Messages.HelloWorld;
    }
}
