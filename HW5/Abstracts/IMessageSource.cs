using System.Net;
using HW5.Models;

namespace HW5.Abstracts;

public interface IMessageSource
{
    Task SendAsync(NetMessage message , IPEndPoint ep);

    NetMessage Receive(ref IPEndPoint ep);
}