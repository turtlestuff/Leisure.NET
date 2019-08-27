using System.Collections.Generic;

namespace Leisure
{
    /// <summary>
    /// Information about the game the user is in and their current game.
    /// </summary>
    public class GameCollection
    {
        public GameCollection()
        {
        }

        /// <summary>
        /// The games which the user is in.
        /// </summary>
        public HashSet<GameInstance> Games { get; } = new HashSet<GameInstance>();

        /// <summary>
        /// The game the user is currently interacting with.
        /// </summary>
        public GameInstance CurrentGame = default!;

        /// <inheritdoc />
        public override string ToString()
        {
            return $"({CurrentGame.Id})";
        }
    }
}