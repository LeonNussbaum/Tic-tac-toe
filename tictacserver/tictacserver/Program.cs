using WebSocketSharp;

using WebSocketSharp.Server;
using System.Collections.Generic;
namespace tictacserver
{
    public class Echo : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Send(e.Data);
            
        }
    }

    public class CreateGame : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Send(e.Data);
        }
    }
    public class JoinGame : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Send(e.Data);
        }
    }
    internal class Program
    {
        public static List<gameinstance> gameinstances = new List<gameinstance>();

        static void Main(string[] args)
        {

            WebSocketServer server = new WebSocketServer("ws://localhost:4242");
            server.Start();

            server.AddWebSocketService<Echo>("/join");

            Console.WriteLine("[SERVER] Started!");
            Console.ReadLine();
            server.Stop();
        }
    }
}
