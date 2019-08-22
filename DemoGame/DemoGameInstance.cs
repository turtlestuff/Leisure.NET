
using System.Collections.Immutable;
using Discord;
using Leisure;

namespace DemoGame
{
    public class DemoGameInstance : GameInstance
    {
        public DemoGameInstance(uint id, ImmutableArray<IUser> players) : base(id, players)
        {
        }
        
        public override void Initialize()
        {
            Broadcast("Ready!");
        }

        public override void OnMessage(IUserMessage message)
        {
            Broadcast(message.Content);
        }
    }
}