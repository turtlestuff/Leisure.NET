
using System.Collections.Immutable;
using System.Threading.Tasks;
using Discord;
using Leisure;

namespace DemoGame
{
    public class DemoGameInstance : GameInstance
    {
        public DemoGameInstance(uint id, ImmutableArray<IUser> players) : base(id, players)
        {
        }
        
        public override async Task Initialize()
        {
            await Broadcast("Ready!");
        }

        public override async Task OnMessage(IUserMessage message)
        {
            await Broadcast(message.Content);
        }
    }
}