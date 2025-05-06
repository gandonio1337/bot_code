using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.bot.Services
{
    public class FileStorageService
    {
        private readonly DiscordSocketClient _client;
        private readonly ulong _allowedUserId;

        public FileStorageService(DiscordSocketClient client, ulong allowedUserId)
        {
            _client = client;
            _allowedUserId = allowedUserId;
        }

        public async Task WaitForFileUpload(SocketInteractionContext context, Func<IAttachment, string, Task> onUpload)
        {
            var channel = context.Channel;

            await channel.SendMessageAsync("Please upload the update file (with an attachment). You have 60 seconds.");

            // Wait for file message
            var fileMessage = await _client.NextMessageAsync(msg =>
                msg.Author.Id == _allowedUserId &&
                msg.Channel.Id == context.Channel.Id &&
                msg.Attachments.Count > 0, timeout: TimeSpan.FromSeconds(60));

            if (fileMessage == null)
            {
                await channel.SendMessageAsync("Timed out waiting for the file upload.");
                return;
            }

            var attachment = fileMessage.Attachments.First();

            await channel.SendMessageAsync("Got the file. Now please send the changelog text. You have 60 seconds.");

            // Wait for changelog message
            var changelogMessage = await _client.NextMessageAsync(msg =>
                msg.Author.Id == _allowedUserId &&
                msg.Channel.Id == context.Channel.Id &&
                msg.Attachments.Count == 0, timeout: TimeSpan.FromSeconds(60));

            if (changelogMessage == null)
            {
                await channel.SendMessageAsync("Timed out waiting for changelog.");
                return;
            }

            string changelog = changelogMessage.Content;

            await onUpload(attachment, changelog);
        }
    }
}
