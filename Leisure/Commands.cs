using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Leisure
{
    static partial class Program
    {
        static async Task ParseGuildMessage(SocketUserMessage msg)
        {
            int pos = 0;
            // Is this a leisure command?
            if (msg.HasStringPrefix("leisure:", ref pos))
            {
                await ParseLeisureCommand(msg, pos);
            }

            // Search through installed installedGames to find one with the prefix. If one is found, try and parse the command
            var game = installedGames.FirstOrDefault(game => msg.HasStringPrefix(game.Prefix + ":", ref pos));
            if (game != default)
            {
                await ParseGameCommand(game, msg, pos);
            }
        }
        
        static async Task ParseGameCommand(GameInfo game, SocketUserMessage msg, int pos)
        {
            switch (msg.Content[pos..])
            {
                case "join":
                    if (startingGames.TryGetValue(msg.Channel, out var startingGame))
                        await JoinExistingLobby(game, msg, startingGame);
                    else
                        await StartNewLobby(game, msg);

                    return;
                case "close" when startingGames.TryGetValue(msg.Channel, out var newGame) 
                                  && newGame.StartingUser == msg.Author:
                    await CloseLobby(game, msg, newGame);
                    return;
                default:
                    return;
            }
        }

        static async Task CloseLobby(GameInfo game, SocketUserMessage msg, GameLobby newGame)
        {
            if (!game.IsValidPlayerCount(newGame.Players.Count))
            {
                await msg.Channel.SendMessageAsync("Not a valid amount of players for this game. Game requires: " +
                                                   game.PlayerCountDescription);
            }
            else
            {
                await msg.Channel.SendMessageAsync("Starting game");
                var inst = game.CreateGame(newGame.Id, newGame.Players);
                foreach (var p in newGame.Players) 
                {
                    if (playingUsers.ContainsKey(p.Id))
                    {
                        playingUsers[p.Id].Games.Add(inst);
                    }
                    else
                    {
                        playingUsers.Add(p.Id,new GameCollection(inst));
                    }
                }
            }
                
            
            startingGames.Remove(msg.Channel);
        }

        static async Task JoinExistingLobby(GameInfo game, SocketUserMessage msg, GameLobby gameLobby)
        {
            if (gameLobby.Players.Add(msg.Author))
                await msg.Channel.SendMessageAsync("Joined game");
            else
                await msg.Channel.SendMessageAsync("You already joined this game");
        }

        static async Task StartNewLobby(GameInfo game, SocketUserMessage msg)
        {
            var number = Interlocked.Add(ref gameCount, 1);
            var newGame = new GameLobby(number, msg.Author, msg.Channel, game);
            startingGames.Add(msg.Channel, newGame);
            await msg.Channel.SendMessageAsync("Opening game " + number.ToString());
        }

        static async Task ParseLeisureCommand(SocketUserMessage msg, int pos)
        {
            switch (msg.Content[pos..])
            {
                case "help":
                    await SendHelp(msg);
                    return;
                case "about":
                    await msg.Channel.SendMessageAsync("This is the about.");
                    return;
                case "games":
                    await SendGames(msg);
                    return;
                case "ping":    
                    await msg.Channel.SendMessageAsync("Pong!");
                    return;
                case var gameName when installedGames.FirstOrDefault(g => String.Equals(g.Prefix, gameName, StringComparison.Ordinal)) is GameInfo game:
                    await SendGame(game, msg);
                    return;
                default:
                    return;
            }
        }

        static async Task SendHelp(SocketUserMessage msg)
        {
            var builder = new EmbedBuilder
            {
                Title = "Leisure Help",
                Description = @"**leisure:ping**: Makes sure the bot is online
**leisure:help**: Gets this message
**leisure:games**: Gets the list of the available games
**leisure:<game prefix>**: Gets information about the specified game"
            };
            
            await msg.Channel.SendMessageAsync("", embed: builder.Build());
        }

        static async Task SendGame(GameInfo game, SocketUserMessage msg)
        {
            var builder = new EmbedBuilder
            {
                Title = $"**{game.Name}** ({game.Prefix}:)",
                Description = $@"{game.Description}

**Author(s)**: {game.Author}
**Version:** {game.Version},
**Player Requirements:** {game.PlayerCountDescription}"
            };
            
            await msg.Channel.SendMessageAsync("", embed: builder.Build());
        }

        static async Task SendGames(SocketUserMessage msg)
        {
            var builder = new EmbedBuilder
            {
                Title = "Available Games",
                Description = string.Join("\n", installedGames.Select(game => $"**{game.Name}** ({game.Prefix}:)"))
            };
            
            await msg.Channel.SendMessageAsync("", embed: builder.Build());
        }
    }
}