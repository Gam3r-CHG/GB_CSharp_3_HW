using HW7.Models;
using NetMQ.Sockets;

namespace HW7.Abstracts;

public interface IMessageSource
{
    // Упростил для наглядности, чтобы не делать два интефейса
    void SendClient(NetMessage message);

    NetMessage ReceiveServer(RouterSocket server);
}