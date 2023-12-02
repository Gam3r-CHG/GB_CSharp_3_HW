namespace HW2_Client;

internal static class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 1 && Int32.TryParse(args[0], out int port))
        {
            new ChatClient(port);
        }
        else if (args.Length == 2)
        {
            new ChatClient(from: args[0], ipAddress: args[1]);
        }
        else
        {
            new ChatClient();
        }
    }
}