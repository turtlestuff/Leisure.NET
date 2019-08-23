using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
        protected async Task Broadcast(string text, bool isTTS = false, Embed embed = default)
        {
            Parallel.ForEach(Players, async player => await player.SendMessageAsync(text, isTTS, embed));
        }
        
        /// <summary>
        /// Broadcasts a message to every user  n <paramref name="players"/>.
        /// </summary>
        /// <param name="text">Text to broadcast</param>
        /// <param name="isTTS">Message send as TTS? (default is false)</param>
        /// <param name="embed">Embed to send (default is none)</param>
        /// <param name="players">Users to send.</param>
        protected async Task BroadcastTo(string text, bool isTTS = false, Embed embed = default, params IUser[] players)
        {
            Parallel.ForEach(players, async player => await player.SendMessageAsync(text, isTTS, embed));
        }

        /// <summary>
        /// Broadcasts a message to every user not in <paramref name="exclude"/>.
        /// </summary>
        /// <param name="text">Text to broadcast</param>
        /// <param name="isTTS">Message send as TTS? (default is false)</param>
        /// <param name="embed">Embed to send (default is none)</param>
        /// <param name="exclude">Users to exclude.</param>
        protected async Task BroadcastExcluding(string text, bool isTTS = false, Embed embed = default,
            params IUser[] exclude)
        {
            Parallel.ForEach(Players.Except(exclude), async player => await player.SendMessageAsync(text, isTTS, embed));
        }
    }
}