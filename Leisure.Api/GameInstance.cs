using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Discord;

namespace Leisure
{
    /// <summary>
    /// Represents an instance of a game.
    /// </summary>
    public abstract class GameInstance
    {
        /// <summary>
        /// Creates a new instance of a game.
        /// </summary>
        /// <param name="id">The ID of the new game.</param>
        /// <param name="players">The players that are in the game.</param>
        protected GameInstance(int id, HashSet<IUser> players)
        {
            Id = id;
            Players = players;
        }
        
        /// <summary>
        /// Gets the players playing the game.
        /// </summary>
        public HashSet<IUser> Players { get; }
        
        /// <summary>
        /// Get the ID of the game.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// This is ran when the game has been closed, and is ready to be initialized.
        /// </summary>
        public abstract Task Initialize();
        
        /// <summary>
        /// Ran when Leisure.NET detects a message for your game.
        /// </summary>
        /// <param name="message">Message sent</param>
        public abstract Task OnMessage(IUserMessage message);

        /// <summary>
        /// Broadcasts a message to every user in the game.
        /// </summary>
        /// <param name="text">Text to broadcast</param>
        /// <param name="isTTS">Message send as TTS? (default is false)</param>
        /// <param name="embed">Embed to send (default is none)</param>
        /// <returns></returns>
        protected async Task Broadcast(string text, bool isTTS = false, Embed? embed = default)
        {
            foreach (var player in Players) 
            {
                await player.SendMessageAsync(text, isTTS, embed);
            }
        }
        
        /// <summary>
        /// Broadcasts a message to every user  n <paramref name="players"/>.
        /// </summary>
        /// <param name="text">Text to broadcast</param>
        /// <param name="isTTS">Message send as TTS? (default is false)</param>
        /// <param name="embed">Embed to send (default is none)</param>
        /// <param name="players">Users to send.</param>
        protected async Task BroadcastTo(string text, bool isTTS = false, Embed? embed = default, params IUser[] players)
        {
            foreach (var player in players) 
            {
                await player.SendMessageAsync(text, isTTS, embed);
            }
        }

        /// <summary>
        /// Broadcasts a message to every user not in <paramref name="exclude"/>.
        /// </summary>
        /// <param name="text">Text to broadcast</param>
        /// <param name="isTTS">Message send as TTS? (default is false)</param>
        /// <param name="embed">Embed to send (default is none)</param>
        /// <param name="exclude">Users to exclude.</param>
        protected async Task BroadcastExcluding(string text, bool isTTS = false, Embed? embed = default,
            params IUser[] exclude)
        {
            foreach (var player in Players.Except(exclude)) 
            {
                await player.SendMessageAsync(text, isTTS, embed);
            }
        }
    }
}