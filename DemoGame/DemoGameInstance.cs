using System.Collections.Generic;
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

        public override async Task OnMessage(IUserMessage msg, int pos)
        {
            await BroadcastExcluding($"{msg.Author.Username}: {msg.Content}", exclude: msg.Author);
            await msg.AddReactionAsync(new Emoji("✅"));
        }
    }
}