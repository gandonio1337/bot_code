using Discord;
using Discord.WebSocket;
using MyApp.bot.Commands;
using System;
using System.Threading.Tasks;

namespace MyApp.bot
{
    public class BotService
    {
        private readonly DiscordSocketClient _client;
        private readonly UpdateCommand _updateCommand;
        private readonly DownloadCommand _downloadCommand;

        public BotService()
        {
            _client = new DiscordSocketClient();
            _updateCommand = new UpdateCommand();
            _downloadCommand = new DownloadCommand();
        }

        public async Task StartAsync(string token)
        {
            _client.Log += LogAsync;
            _client.Ready += ReadyAsync;
            _client.SlashCommandExecuted += HandleSlashCommandAsync;

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private async Task ReadyAsync()
        {
            var guild = _client.Guilds.First();
            await guild.CreateApplicationCommandAsync(new SlashCommandBuilder()
                .WithName("update")
                .WithDescription("Upload a new loader version")
                .Build());

            await guild.CreateApplicationCommandAsync(new SlashCommandBuilder()
                .WithName("download")
                .WithDescription("Download the latest loader")
                .Build());
        }

        private async Task HandleSlashCommandAsync(SocketSlashCommand command)
        {
            switch (command.CommandName)
            {
                case "update":
                    await _updateCommand.ExecuteAsync(command);
                    break;
                case "download":
                    await _downloadCommand.ExecuteAsync(command);
                    break;
            }
        }
    }
}
