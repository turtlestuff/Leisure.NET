
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
        
        public async override Task Initialize()
        {
            Broadcast("Ready!");
        }

        public override void OnMessage(IUserMessage message)
        {
            Broadcast(message.Content);
        }
    }
}