using System;
using System.Threading.Tasks;
using Untis.Net;

namespace webuntis
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var client = new UntisClient("BK Georg-Simon-Ohm Köln", "hektor.webuntis.com");
            
            // Log in first
            await client.LoginAsync("iaf11nussbaumleo", "i1Yjr8Nn8pnyjrwAPUk5!");

            // Once logged in, you can make other requests
            var stundenplan = await client.GetRoomsAsync();

            // Process the result
            Console.WriteLine("Rooms:");
            await Console.Out.WriteLineAsync(stundenplan.ToString());
        }
    }
}
