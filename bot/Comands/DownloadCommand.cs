using Discord;
using Discord.WebSocket;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.bot.Commands
{
    public class DownloadCommand
    {
        public async Task ExecuteAsync(SocketSlashCommand command)
        {
            var latestFile = Directory.GetFiles("loader_builds")
                .Select(f => new FileInfo(f))
                .OrderByDescending(f => f.CreationTime)
                .FirstOrDefault();

            if (latestFile == null)
            {
                await command.RespondAsync("No loader builds found.", ephemeral: true);
                return;
            }

            await command.RespondWithFileAsync(latestFile.FullName, "Here is the latest loader build:");
        }
    }
}
