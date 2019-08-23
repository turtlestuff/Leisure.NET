using System;
using System.Collections.Immutable;
using Discord;

namespace Leisure
{
    public abstract class GameInfo
    {
        /// <summary>
        /// The full name of the game, that will be seen by the players.
        /// </summary>
        public abstract string Name { get; }
        
        /// <summary>
        /// A description for the game, that will be seen by players if they wish.
        /// </summary>
        public abstract string Description { get; }
        
        /// <summary>
        /// The version of the game. 
        /// </summary>
        public abstract Version Version { get; }
        
        /// <summary>
        /// The author(s) of the games.
        /// </summary>
        public abstract string Author { get; }
        
        /// <summary>
        /// A short description of the amount of players a game can accept.
        /// </summary>
        public abstract string NumOfPlayers { get; }

        /// <summary>
        /// A short, perhaps acronymified name used for joining the game.
        /// </summary>
        public abstract string Prefix { get; }
        
        /// <summary>
        /// Checks whether a certain number of players is valid for a game.
        /// </summary>
        /// <param name="i">Amount of players to check.</param>
        /// <returns>True if the amount is valid.</returns>
        public abstract bool IsValidPlayerCount(uint i);
        
        /// <summary>
        /// Creates the game.
        /// </summary>
        /// <param name="id">The ID of the game.</param>
        /// <param name="players">The <see cref="System.Collections.Immutable.ImmutableArray"/> of players.</param>
        /// <returns>The new <see cref="Leisure.GameInstance"/></returns>
        public abstract GameInstance CreateGame(uint id, ImmutableArray<IUser> players);
    }
}