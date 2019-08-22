using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Discord;

namespace Leisure
{
    public abstract class GameInstance
    {
        protected GameInstance(uint id, ImmutableArray<IUser> players)
        {
            Id = id;
            Players = players;
        }
        
        public ImmutableArray<IUser> Players { get; }
        public uint Id { get; }

        public abstract void Initialize();
        public abstract void OnMessage(IUserMessage message);

        protected async Task Broadcast(string text, bool isTTS = false, Embed embed = default)
        {
            Parallel.ForEach(Players, async player => await player.SendMessageAsync(text, isTTS, embed));
        }
    }
}