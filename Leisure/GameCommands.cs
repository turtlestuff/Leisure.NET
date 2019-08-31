using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;

namespace Leisure
{
    static partial class Program
    {
        static Task ParseDmMessage(IUserMessage msg)
        {
            var pos = 0;
            if (char.IsNumber(msg.Content.FirstOrDefault()))
            {
                var message = msg.Content.AsSpan();
                bool whitespace = false;
                foreach (var c in message)
                {
                    if (char.IsNumber(c))
                        if (whitespace)
                            break;
                        else
                            pos++;

                    if (char.IsWhiteSpace(c))
                    {
                        pos++;
                        whitespace = true;
                    }
                }
                
                if (int.TryParse(message[..pos], out var id))
                {
                    if (!PlayingUsers[msg.Author].Games.TryGetValue(id, out var game))
                        throw new ArgumentException("Provided ID is not a valid game ID.");

                    PlayingUsers[msg.Author].CurrentGame = game;
                }
            }

            return PlayingUsers[msg.Author].CurrentGame.OnMessage(msg, pos);
        }
        
        static void CloseGame(object? sender, EventArgs e)
        {
            var game = (GameInstance) sender!;
            foreach (var p in game.Players)
            {
                var gc = PlayingUsers[p];
                gc.Games.TryRemove(game.Id, out _);

                if (gc.Games.Count == 0)
                { 
                    PlayingUsers.TryRemove(p, out _);
                    p.SendMessageAsync("You are in no more games. Thanks for playing with Leisure!");
                }
                else
                {
                    gc.CurrentGame = gc.Games.Last().Value;
                    p.SendMessageAsync("You have been placed into game " + gc.CurrentGame.Id.ToString());
                }    
            }

            game.Players.Clear();
        }
    }
}