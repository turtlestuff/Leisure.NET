using System.Collections.Generic;
using Discord;

namespace Leisure.NET
{
    public abstract class GameInfo<TGame> where TGame : Game, new()
    {
        
        public abstract string Name { get; }
        public abstract string Prefix { get; }
        public abstract int MaxPlayers { get; }

        public TGame CreateGame(uint id, List<IUser> players)
        {
           TGame game = new TGame();
           game.Players = players;
           game.Id = id;
           game.Initialize();
           return game;
        }
        
    }
}