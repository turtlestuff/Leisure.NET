using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Discord;

namespace Leisure
{
    public abstract class GameInfo
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract Version Version { get; }
        public abstract string Author { get; }


        public abstract string Prefix { get; }
        public abstract int MaxPlayers { get; }
        public abstract GameInstance CreateGame(uint id, ImmutableArray<IUser> players);
    }
}