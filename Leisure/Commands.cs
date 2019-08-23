using System;
using System.Linq;
using System.Threading.Tasks;
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
                ParseLeisureCommand(msg, ref pos);
            }

            var game = games.FirstOrDefault(game => msg.HasStringPrefix("game.Prefix" + ":", ref pos));
            if (game != default)
            {
                await msg.Channel.SendMessageAsync("Game: " + game.Name);
            }
        }
        
        static void ParseLeisureCommand(SocketUserMessage msg, int pos)
        {
            return;
        }
    }
}