using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Discord_BOT
{
    class Program
    {   //ver 1.0 2019/03/03

        public static DiscordSocketClient client;
        public static CommandService commands;
        public static IServiceProvider services;
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
        readonly char prefix = ':';

        public async Task MainAsync()
        {
            var version = "1.0";
            client = new DiscordSocketClient();
            commands = new CommandService();
            services = new ServiceCollection().BuildServiceProvider();

            client.MessageReceived += CommandRecieved;

            client.Log += Log;

            IniFile ini = new IniFile("./Setting/Setting.ini");
            var token = ini["section", "bot_token"];

            await commands.AddModulesAsync(Assembly.GetEntryAssembly());
            await client.SetGameAsync($":help | ver {version}");
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            await Task.Delay(-1);
        }

        private async Task CommandRecieved(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;

            Console.WriteLine("{0} {1}:{2}", message.Channel.Name, message.Author.Username, message);

            if (message == null) { return; }

            if (message.Author.IsBot) { return; }

            int argPos = 0;

            var context = new CommandContext(client, message);

            if (!(message.HasCharPrefix(prefix, ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos)))
            {
                return;
            }


            var result = await commands.ExecuteAsync(context, argPos, services);

            if (!result.IsSuccess)
            {
                await context.Channel.SendMessageAsync(result.ErrorReason);
            }
        }

        private Task Log2(string msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
