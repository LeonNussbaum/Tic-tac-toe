using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace tictacserver
{
    public class Gameinstance
    {
        public List<WebSocketBehavior> Players { get; private set; }
        public  Gameinstance()
        {

            string generatedid;
            bool validid = false;
            do
            {
                generatedid = "";
                generatedid = genid();
                
                validid = checkifidisfree(Program.gameinstances, generatedid);
            } while (!validid);
            this.id = generatedid;
            Players = new List<WebSocketBehavior>();
        }

        private string genid()
        {
            Random rnd = new Random();
            string gid = "";
            for (int i = 0; i < 4; i++)
            {
                gid += Convert.ToString(rnd.Next(0, 9));
            }
            return gid;
        }

        private bool checkifidisfree(List<Gameinstance> gameinstances, string gid)
        {
            for (int i = 0; i < gameinstances.Count; i++)
            {
                if (gameinstances[i].id == gid)
                {
                    return false;
                }
            }
            return true;
        }

        public static string GetCurrentSessionsById()
        {
            string r = "p;";

            for (int i = 0; i < Program.gameinstances.Count; i++)
            {
                r += Program.gameinstances[i].id + ";";
            }
            Console.WriteLine(r);
            return r;
            
        }

        public string id;

        public string[,] gamesymbols = new string[3, 3];


        public bool playerx;

        public bool playero;

        public string Wincheck()
        {
            for (int i = 0; i < 3; i++)

            {
                int xcount = 0;
                int ocount = 0;
                for (int j = 0; j < 3; j++)
                {
                    if (gamesymbols[j, i] != null)
                    {
                        if (gamesymbols[j, i] == "X")
                        {
                            xcount++;
                        }
                        if (gamesymbols[j, i] == "O")
                        {
                            ocount++;
                        }
                    }
                }
                if (xcount == 3)
                {
                    return "X";
                }
                if (ocount == 3)
                {
                    return "O";
                }
            }
            for (int i = 0; i < 3; i++)
            {
                int xcount = 0;
                int ocount = 0;
                for (int j = 0; j < 3; j++)
                {
                    if (gamesymbols[i, j] != null)
                    {
                        if (gamesymbols[i, j] == "X")
                        {
                            xcount++;
                        }
                        if (gamesymbols[i, j] == "O")
                        {
                            ocount++;
                        }
                    }
                }
                if (xcount == 3)
                {
                    return "X";
                }
                if (ocount == 3)
                {
                    return "O";
                }
            }
            int xcountD1 = 0;
            int ocountD1 = 0;
            int xcountD2 = 0;
            int ocountD2 = 0;
            for (int i = 0; i < 3; i++)
            {
                if (gamesymbols[i, i] != null)
                {
                    if (gamesymbols[i, i] == "X")
                    {
                        xcountD1++;
                    }
                    else if (gamesymbols[i, i] == "O")
                    {
                        ocountD1++;
                    }
                }
                if (gamesymbols[i, 2 - i] != null)
                {
                    if (gamesymbols[i, 2 - i] == "X")
                    {
                        xcountD2++;
                    }
                    else if (gamesymbols[i, 2 - i] == "O")
                    {
                        ocountD2++;
                    }
                }
            }
            if (xcountD1 == 3 || xcountD2 == 3)
            {
                return "X";
            }
            else if (ocountD1 == 3 || ocountD2 == 3)
            {
                return "O";
            }
            bool boardFull = true;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (gamesymbols[i, j] == null)
                    {
                        boardFull = false;
                        break;
                    }
                }
                if (!boardFull)
                    break;
            }
            if (boardFull)
            {
                return "XO";
            }
            return "";
        }
    }
}


