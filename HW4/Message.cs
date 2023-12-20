using System.Text.Json;

namespace HW4;

public class Message
{
    public DateTime DateTime { get; set; }
    public string Text { get; set; } = "";
    public string From { get; set; } = "";
    public string To { get; set; } = "";

    public override string ToString()
    {
        return $"[{DateTime}] From: {From}. To: {To}. Message: {Text}";
    }

    public string SerializeToJson()
    {
        var jsonString = JsonSerializer.Serialize(this, new JsonSerializerOptions {WriteIndented = true});
        return jsonString;
    }

    public static Message DeserializeFromJson(string json)
    {
        var obj = JsonSerializer.Deserialize<Message>(json);
        return obj;
    }

    // For pattern "Prototype"
    public Message Clone()
    {
        return new Message {DateTime = DateTime, From = From, To = To, Text = Text};
    }
}