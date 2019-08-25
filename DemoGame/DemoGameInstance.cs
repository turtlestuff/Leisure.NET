
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Discord;
using Leisure;

namespace DemoGame
{
    public class DemoGameInstance : GameInstance
    {
        public DemoGameInstance(int id, HashSet<IUser> players) : base(id, players)
        {
        }
        
        public override async Task Initialize()
        {
            await Broadcast("Ready!");
        }

        public override async Task OnMessage(string command, string args, IUserMessage msg)
        {
            await BroadcastTo(command, false, default, msg.Author);
            await BroadcastExcluding($"{msg.Author.Username}: {command}", false, 
                default, msg.Author);
        }
    }
}