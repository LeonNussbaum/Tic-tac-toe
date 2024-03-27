using System.Security.Cryptography.X509Certificates;
using WebSocketSharp;


namespace tictacconsole
{
    internal class Program
    {
        public static List<string> Sessions = new List<string>();
        public static WebSocket webSocket = new WebSocket("ws://localhost:4242/join");
        public static string[,] gridcoppy = new string[3,3];
        public static string[,] Gamegrid = new string[3,3];
        public static string winuser = "";
        public static bool Gamestart = false;

        public static string playertype = string.Empty;

        public static bool isturn;

        static void Main(string[] args)
        {

            webSocket.Connect();

            webSocket.OnMessage += WebSocket_OnMessage;
            
            Menu();

        }

        public static void Menu()
        {
            int select = selection();
            if (select ==1)
            {
                Console.Clear();
                webSocket.Send("P");
                Thread.Sleep(30);
                for (int i = 0; i < Sessions.Count; i++)
                {
                    Console.WriteLine(Sessions[i]);
                }
                Console.ReadLine();
                Menu();

            }
            else if (select == 2)
            {
                if (Sessions.Count == 0)
                {
                    webSocket.Send("P");
                    Thread.Sleep(30);
                }
                string id = "";
                while (true)
                {
                    webSocket.Send("P");
                    Thread.Sleep(30);
                    Console.Write("Enter Session ID :");
                    id = Console.ReadLine();
                    if (Sessions.Contains(id))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Id invalid");
                    }
                }

                webSocket.Send("J" + id);
                Console.WriteLine("Waiting for other players");
                while (!Gamestart)
                {
                }
                Gameloop();



            }
            else if (select ==3)
            {
                webSocket.Send("C");
                Console.Clear();
                Menu();
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

        public static void Gameloop()
        {
            Console.Clear();
            int x =0, y=0;

            while (winuser == "")
            {
                if (Console.KeyAvailable)
                {
                    string key = Console.ReadKey().Key.ToString();

                    if (key == "UpArrow")
                    {
                        y--;
                    }
                    else if (key == "RightArrow")
                    {
                        x++;
                    }
                    else if (key == "LeftArrow")
                    {
                        x--;
                    }
                    else if (key == "DownArrow")
                    {
                        y++;
                    }
                    else if (key == "Enter")
                    {
                        if (Gamegrid[x, y] == " ")
                        {
                            if (isturn)
                            {
                                webSocket.Send("GD" + ";" + x + ";" + y);
                            }
                        }
                    }
                    Console.Clear();
                    PrintGrid(x, y);
                    
                }
                Thread.Sleep(200);
                if (gridcoppy != Program.Gamegrid)
                {
                    gridcoppy = Program.Gamegrid;
                    Console.Clear();
                    PrintGrid(x, y);
                }
            }
            Console.Clear();
            Console.WriteLine(winuser);
            Console.ReadKey();
            winuser = "";
            Gamestart = false;
            Menu();
        }

        public static void PrintGrid(int x, int y)
        {
            Console.WriteLine("-------");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (j == x && i == y)
                    {
                        Console.Write("|");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.Write(Gamegrid[j, i]);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else 
                    {                      
                        Console.Write("|");
                        Console.Write(Gamegrid[j, i]);
                    }
                }
                Console.Write("|");
                Console.WriteLine();
                Console.WriteLine("-------");
            }
        }

        public static string[,] GameBuilder(string input)
        {
            string[] data = input.Split(";");
            string[,] retundata = new string[data.Length,3];
            for (int i = 0; i < 3; i++)
            {
                
                for (int j = 0; j < 3; j++)
                {
                    retundata[j,i] = Convert.ToString(data[i][j]);
                }
                
            }
            return retundata;
        }

        private static void WebSocket_OnMessage(object? sender, MessageEventArgs e)
        {
            
            if (e.Data.StartsWith('p'))
            {
                string input = e.Data;
                Sessions.Clear();
                string[] data = input.Split(";");
                
                for (int i = 0; i < data.Length; i++)
                {
                    if (i != 0)
                    {
                        Sessions.Add(data[i]);
                        //Console.Write(data[i]);
                    }
                }
                if (data.Length == 2)
                {
                    Sessions.Add("No Instances Available");
                    //Console.WriteLine("No Instances Available");
                    
                }
            }
            else if (e.Data.StartsWith("STARTP"))
            {
                string input = e.Data.Substring(6);
                Gamegrid = GameBuilder(input);

                isturn = true;
                Gamestart = true;
                

            }
            else if (e.Data.StartsWith("STARTW"))
            {
                string input = e.Data.Substring(6);
                Gamegrid = GameBuilder(input);
                isturn = false;
                Gamestart = true;

            }
            else if (e.Data.StartsWith("C"))
            {
                playertype = e.Data.Substring(1);
            }
            else if (e.Data.StartsWith('F'))
            {
                Console.WriteLine(e.Data);
            }
            else if (e.Data.StartsWith("W"))
            {
                winuser = "Player with the " + e.Data.Substring(1)+ " winns";
            }
            else
            {
                Console.WriteLine(e.Data);
            }
        }
    }
}
