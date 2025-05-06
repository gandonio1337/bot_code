using Discord;
using Discord.WebSocket;
using MyApp.bot.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MyApp.bot.Commands
{
    public class UpdateCommand
    {
        private readonly FileStorageService _fileService = new FileStorageService();
        private const ulong OwnerId = 1162083978308751390;
        private const ulong UploadChannelId = 1369312668594475118; // Replace

        public async Task ExecuteAsync(SocketSlashCommand command)
        {
            if (command.User.Id != OwnerId)
            {
                await command.RespondAsync("You are not authorized to use this command.", ephemeral: true);
                return;
            }

            await command.RespondAsync("Please upload the loader file as an attachment.");

            _fileService.WaitForFileUpload(async (file, changelog) =>
            {
                var path = Path.Combine("loader_builds", file.Filename);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var client = command.Client as DiscordSocketClient;
                var channel = client.GetChannel(UploadChannelId) as IMessageChannel;

                var message = new FileAttachment(path);
                await channel.SendFileAsync(message, $"New loader uploaded by {command.User.Username}\nChangelog:\n{changelog}");
            });
        }
    }
}
