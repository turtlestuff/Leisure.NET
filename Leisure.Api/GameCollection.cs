using System.Collections.Immutable;

namespace Leisure
{
    public class GameCollection
    {
        public ImmutableArray<GameInstance> Games { get; }
        public GameInstance CurrentGame { get; }
    }
}