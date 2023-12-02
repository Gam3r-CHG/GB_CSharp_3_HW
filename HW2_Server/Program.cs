namespace HW2_Server;

internal static class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 1 && Int32.TryParse(args[0], out int port))
        {
            new ChatServer(port);
        }
        else
        {
            new ChatServer();
        }
    }
}