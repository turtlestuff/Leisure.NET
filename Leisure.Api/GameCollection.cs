using System.Collections.Generic;

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
            Games = new List<GameInstance>();
            Games.Add(startingGame);
        }

        /// <summary>
        /// The games which the user is in.
        /// </summary>
        public List<GameInstance> Games { get; }

        /// <summary>
        /// The game the user is currently interacting with.
        /// </summary>
        public GameInstance CurrentGame;

        /// <inheritdoc />
        public override string ToString()
        {
            return $"({CurrentGame.Id})";
        }
    }
}