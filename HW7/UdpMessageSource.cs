using HW7.Abstracts;
using HW7.Models;
using NetMQ;
using NetMQ.Sockets;

namespace HW7;

public class UdpMessageSource : IMessageSource
{
    public NetMessage ReceiveServer(RouterSocket server)
    {
        var msg = server.ReceiveMultipartMessage();

        Task.Run(() =>
        {
            var responseMessage = new NetMQMessage();
            responseMessage.Append(msg.First);
            responseMessage.Append("ok");
            server.SendMultipartMessage(responseMessage);
        });

        return NetMessage.DeserializeMessgeFromJSON(msg.Last.ConvertToString());
    }

    public void SendClient(NetMessage message)
    {
        using (var client = new DealerSocket())
        {
            client.Connect($"tcp://127.0.0.1:5557");
            client.SendFrame(message.SerialazeMessageToJSON());

            Task.Run(() =>
            {
                var msg = client.ReceiveFrameString();
                Console.WriteLine($"From server: {msg}");
            });
        }
    }
}