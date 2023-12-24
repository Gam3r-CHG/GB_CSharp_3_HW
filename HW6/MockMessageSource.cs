using System.Net;
using HW6.Abstracts;
using HW6.Models;

namespace HW6;

public class MockMessageSource : IMessageSource
{
    private readonly Queue<NetMessage> _messages = new();
    private Server? _server;
    private readonly IPEndPoint _endPoint = new(IPAddress.Any, 0);

    public MockMessageSource()
    {
        _messages.Enqueue(new NetMessage
            {Command = Command.Register, NickNameFrom = "Max", IpAddress = "127.0.0.1", Port = 12346});
        _messages.Enqueue(new NetMessage
            {Command = Command.Register, NickNameFrom = "Test", IpAddress = "127.0.0.1", Port = 12347});
        _messages.Enqueue(new NetMessage
        {
            Command = Command.Message, NickNameFrom = "Test", NickNameTo = "Max", Text = "Test1",
            IpAddress = "127.0.0.1", Port = 12347
        });
        _messages.Enqueue(new NetMessage
        {
            Command = Command.Message, NickNameFrom = "Max", NickNameTo = "Test", Text = "Test2",
            IpAddress = "127.0.0.1", Port = 12347
        });
    }

    public Task SendAsync(NetMessage message, IPEndPoint ep)
    {
        Console.WriteLine($"Emulate sending message: {message.Text}");
        return Task.Run(() => Thread.Sleep(100));
    }

    public NetMessage Receive(ref IPEndPoint ep)
    {
        ep = _endPoint;
        if (_messages.Count == 0)
        {
            _server.Stop();
            return null;
        }

        return _messages.Dequeue();
    }

    public void AddServer(Server server)
    {
        _server = server;
    }
}