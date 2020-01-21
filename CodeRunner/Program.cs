using CodeRunner.Languages;
using Discord;
using Discord.WebSocket;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CodeRunner
{
    class Program
    {
        public static DiscordSocketClient client;

        //Start the App in and Async Context
        public static void Main(string[] args)
        => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            using(client = new DiscordSocketClient())
            {
                Console.WriteLine("Bot Starting");

                await client.LoginAsync(TokenType.Bot, File.ReadAllText("BotToken").Trim());
                await client.StartAsync();

                client.Log += (msg) => {
                    Console.WriteLine(msg.ToString());
                    return Task.CompletedTask;
                };

                Console.WriteLine("Bot Started");
                client.MessageReceived += Client_MessageReceived;

                await Task.Delay(-1);
            }
        }

        private async Task Client_MessageReceived(SocketMessage arg)
        {
            try
            {
                if (arg.Author.Id != client.CurrentUser.Id)
                {
                    if (arg.Content.StartsWith("coderun js"))
                    {
                        Console.WriteLine("Got JS Command");

                        var results = new jsRunner().Run(arg.Content);
                        if (!string.IsNullOrWhiteSpace(results))
                        {
                            await arg.Channel.SendMessageAsync(results);
                        }
                    }
                    else if (arg.Content == "coderun help")
                    {
                        Console.WriteLine("Got Help Command");

                        await arg.Channel.SendMessageAsync("CodeRun Help:");
                        await arg.Channel.SendMessageAsync("CodeRun will only print the first 500 characters");
                        await arg.Channel.SendMessageAsync("To create a block of code type 3 `");
                        await arg.Channel.SendMessageAsync("Running JS");
                        await arg.Channel.SendMessageAsync("coderun js``` your code here ```");
                    }
                }
            }
            catch(Exception e)
            {
                //Just ignore errors for now
            }
        }
    }
}
