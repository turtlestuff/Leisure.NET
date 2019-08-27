using System;
using System.Collections.Concurrent;
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
        public static DiscordSocketClient Client = default!;

        public static ConcurrentDictionary<IUser, GameCollection> PlayingUsers = new ConcurrentDictionary<IUser, GameCollection>(DiscordComparers.UserComparer);

        static ImmutableArray<GameInfo> InstalledGames;

        static ConcurrentDictionary<IMessageChannel, GameLobby> Lobbies = new ConcurrentDictionary<IMessageChannel, GameLobby>();

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

            // Find all the InstalledGames. The game DLL's name has to currently end in "Game". 
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

            InstalledGames = gameInfos.ToImmutableArray();

            Client = new DiscordSocketClient();
            Client.MessageReceived += ClientOnMessageReceived;
            Client.Log += msg =>
            {
                Console.WriteLine(msg.ToString());
                return Task.CompletedTask;
            };

            await Client.LoginAsync(TokenType.Bot, token);
            await Client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }

        static async Task ClientOnMessageReceived(SocketMessage arg)
        {
            // Not a message sent by a user
            if (!(arg is SocketUserMessage msg))
                return;

            if (msg.Channel is IGuildChannel)
            {
                // We are in a discord server. Try to start new game
                await ParseGuildMessage(msg);
                return;
            }
            
            if (msg.Author != Client.CurrentUser)
                await ParseDmMessage(msg);
        }
    }
}