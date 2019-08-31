using System;
using System.Collections.Generic;
using System.Reflection;
using Discord;

namespace Leisure
{
    /// <summary>
    /// Represents the information about a game.
    /// </summary>
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
        public virtual string Version => GetType().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        /// <summary>
        /// The author(s) of the games.
        /// </summary>
        public abstract string Author { get; }

        /// <summary>
        /// A short description of the amount of players a game can accept.
        /// </summary>
        public abstract string PlayerCountDescription { get; }

        /// <summary>
        /// A short, perhaps acronymified name used for joining the game.
        /// </summary>
        public abstract string Prefix { get; }

        /// <summary>
        /// Checks whether a certain number of players is valid for a game.
        /// </summary>
        /// <param name="i">Amount of players to check.</param>
        /// <returns>True if the amount is valid.</returns>
        public virtual bool IsValidPlayerCount(int i) => true;

        /// <summary>
        /// Creates the game.
        /// </summary>
        /// <param name="client">The client the new game will use.</param>
        /// <param name="players">The players in the new game.</param>
        /// <param name="id">The ID of the game.</param>
        /// <returns>The new <see cref="Leisure.GameInstance"/></returns>
        public abstract GameInstance CreateGame(IDiscordClient client, HashSet<IUser> players, int id);
    }
}