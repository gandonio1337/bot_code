using System;
using System.IO;
using System.Threading.Tasks;
using MyApp.bot;
using dotenv.net;

namespace MyApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            DotEnv.Load();
            string token = Environment.GetEnvironmentVariable("MTM2OTMwNzI4OTMxOTkwMzI5NQ.GkGlCE.EhlCyWLOiMBHL63L_m6k7U23lRkDjYcpMSeWn4");

            var bot = new BotService();
            await bot.StartAsync(token);
        }
    }
}