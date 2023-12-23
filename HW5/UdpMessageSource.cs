using System.Net;
using System.Net.Sockets;
using System.Text;
using HW5.Abstracts;
using HW5.Models;

namespace HW5;

public class UdpMessageSource : IMessageSource
{
    private readonly UdpClient _udpClient;

    public UdpMessageSource()
    {
        _udpClient = new UdpClient(12345);
    }

    public NetMessage Receive(ref IPEndPoint ep)
    {
        byte[] data = _udpClient.Receive(ref ep);
        string str = Encoding.UTF8.GetString(data);
        return NetMessage.DeserializeMessgeFromJSON(str) ?? new NetMessage();
    }

    public async Task SendAsync(NetMessage message, IPEndPoint ep)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(message.SerialazeMessageToJSON());
        await _udpClient.SendAsync(buffer, buffer.Length, ep);
    }
}