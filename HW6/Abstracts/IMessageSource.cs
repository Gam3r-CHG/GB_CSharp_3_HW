using System.Net;
using HW6.Models;

namespace HW6.Abstracts;

public interface IMessageSource
{
    Task SendAsync(NetMessage message, IPEndPoint ep);

    NetMessage Receive(ref IPEndPoint ep);
}