using System.Collections.Immutable;

namespace Leisure
{
    /// <summary>
    /// Information about the game the user is in and their current game.
    /// </summary>
    public class GameCollection
    {
        internal GameCollection(GameInstance startingGame)
        {
            CurrentGame = startingGame;
            var builder = ImmutableArray.CreateBuilder<GameInstance>();
            builder.Add(startingGame);
            Games = builder.ToImmutable();
        }
        
        /// <summary>
        /// The games which the user is in.
        /// </summary>
        public ImmutableArray<GameInstance> Games { get; }
        
        /// <summary>
        /// The game the user is currently interacting with.
        /// </summary>
        public GameInstance CurrentGame { get; }
    }
}