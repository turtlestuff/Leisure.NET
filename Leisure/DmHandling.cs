using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;

namespace Leisure
{
    static partial class Program
    {
        static async Task ParseDmMessage(IUserMessage msg)
        {
            var message = msg.Content.Split(" ");
            if (int.TryParse(message[0], out var index))
            {
                GameInstance gtoset = playingUsers[msg.Author.Id].Games.First(instance => instance.Id == index);
                playingUsers[msg.Author.Id].CurrentGame = gtoset; //TODO: Make this actually work!
                await playingUsers[msg.Author.Id].CurrentGame
                    .OnMessage(message[1], String.Join(" ", message[2..]), msg);
            }
            else
            {
                await playingUsers[msg.Author.Id].CurrentGame
                    .OnMessage(message[0], String.Join(" ", message[1..]), msg);
            }
        }
    }
}