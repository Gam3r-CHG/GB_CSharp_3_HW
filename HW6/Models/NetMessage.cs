using System.Text.Json;

namespace HW6.Models;

public enum Command
{
    Register,
    Message,
    Confirmation
}

public class NetMessage
{
    public int? Id { get; set; }
    public string Text { get; set; }
    public DateTime DateTime { get; set; }
    public string? NickNameFrom { get; set; }
    public string? NickNameTo { get; set; }
    public string IpAddress { get; set; }
    public int Port { get; set; }

    public Command Command { get; set; }

    public string SerialazeMessageToJSON() => JsonSerializer.Serialize(this);

    public static NetMessage? DeserializeMessgeFromJSON(string message) =>
        JsonSerializer.Deserialize<NetMessage>(message);

    public void PrintGetMessageFrom()
    {
        Console.WriteLine(ToString());
    }

    public override string ToString()
    {
        return $"[{DateTime}]({NickNameFrom}): {Text}";
    }
}