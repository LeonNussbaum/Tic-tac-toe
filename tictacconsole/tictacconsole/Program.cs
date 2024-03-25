using System.Security.Cryptography.X509Certificates;
using WebSocketSharp;


namespace tictacconsole
{
    internal class Program
    {
        public static WebSocket webSocket = new WebSocket("ws://localhost:4242/join");

        static void Main(string[] args)
        {

            webSocket.Connect();

            webSocket.OnMessage += WebSocket_OnMessage;


            Menu();

            Console.ReadKey();



        }

        public static void Menu()
        {
            int select = selection();
            if (select ==1)
            {
                webSocket.Send("P");
            }
            else if (select == 2)
            {
               
            }
            else if (select ==3)
            {
                webSocket.Send("C");
            }
        }

        public static void DrawMenu()
        {
            Console.WriteLine("1. Print all available sessions");
            Console.WriteLine("2. Join Session");
            Console.WriteLine("3. Create Session");
            Console.Write(": ");
        }
        public static int selection()
        {
            
            while (true)
            {
                DrawMenu();
                string select = Console.ReadLine();
                if (select == "1" || select == "2" || select == "3")
                {
                    return Convert.ToInt32(select);
                }
                else
                {
                    Console.Clear();
                }
            }
        }


        private static void WebSocket_OnMessage(object? sender, MessageEventArgs e)
        {
            if (e.Data.StartsWith('p'))
            {
                string input = e.Data;

                string[] data = input.Split(";");
                
                for (int i = 0; i < data.Length; i++)
                {
                    if (i != 0)
                    {
                        Console.Write(data[i]);
                    }
                }
                if (data.Length == 2)
                {
                    Console.WriteLine("No Instances Available");
                }
            }
        }
    }
}
