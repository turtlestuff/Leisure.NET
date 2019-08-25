using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Leisure
{
    static partial class Program
    {
        static DiscordSocketClient client = default!;

        static Dictionary<ulong, GameCollection> playingUsers = new Dictionary<ulong, GameCollection>();

        static ImmutableArray<GameInfo> installedGames;

        static Dictionary<IMessageChannel, GameLobby> startingGames = new Dictionary<IMessageChannel, GameLobby>();

        static int gameCount;
        
        static async Task Main()
        {
            string token;
            if (!File.Exists("token.txt"))
            {
                Console.WriteLine("\"token.txt\" doesn't exist. Enter token:");
                token = Console.ReadLine();
                Console.WriteLine("Create token.txt? [Y/n]");
                if (Console.ReadKey().Key != ConsoleKey.N)
                {
                    await using var file = File.CreateText("token.txt");
                    file.Write(token);
                }
            }
            else
            {
                token = File.ReadAllText("token.txt");
            }

            var gameInfos = new List<GameInfo>();

            // Find all the installedGames. The game DLL's name has to currently end in "Game". 
            // TODO: Isolate each game to its own folder
            foreach (var asm in Directory.GetFiles("Games", "*Game.dll", SearchOption.AllDirectories))
            {
                try
                {
                    Console.WriteLine("Loading " + asm);
                    
                    // Load the assembly
                    // TODO: Assembly unloadability
                    var assembly = Assembly.LoadFrom(asm);

                    // Find every class that implements GameInfo and make a new instance of it.
                    gameInfos.AddRange(from t in assembly.ExportedTypes
                        where t.IsSubclassOf(typeof(GameInfo))
                        select (GameInfo) Activator.CreateInstance(t)!);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error loading assembly at {0}: {1}", asm, ex);
                }
            }

            installedGames = gameInfos.ToImmutableArray();

            client = new DiscordSocketClient();
            client.MessageReceived += ClientOnMessageReceived;
            client.Log += msg =>
            {
                Console.WriteLine(msg.ToString());
                return Task.CompletedTask;
            };
            
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            
            await Task.Delay(Timeout.Infinite);
        }

        static async Task ClientOnMessageReceived(SocketMessage arg)
        {
            foreach (var g in playingUsers)
            {
                Console.WriteLine("{0}: Main {1}",g.Key,g.Value);
            }
            // Not a message sent by a user
            if (!(arg is SocketUserMessage msg))    
                return;

            if (msg.Channel is IGuildChannel)
            {    
                // We are in a discord server. Try to start new game
                await ParseGuildMessage(msg);
                return;
            }



            if (msg.Author != client.CurrentUser)
                await ParseDmMessage(msg);

            
            
        }
    }
}