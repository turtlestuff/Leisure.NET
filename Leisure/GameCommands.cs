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
            var splits = msg.Content.Split(" ");
            GameInstance game;
            int pos = 0;
            if (int.TryParse(splits[0], out var id))
            {
                game = PlayingUsers[msg.Author].Games.FirstOrDefault(instance => instance.Id == id);
                
                if (game == default)
                    throw new ArgumentException("Provided ID is not a valid game ID.");

                pos = splits[0].Length - 1;
            }
            else
            {
                game = PlayingUsers[msg.Author].CurrentGame;
            }

            return game.OnMessage(msg, pos);
        }
    }
}