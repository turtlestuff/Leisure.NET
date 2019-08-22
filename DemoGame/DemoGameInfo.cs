using System;
using System.Collections.Immutable;
using Discord;
using Leisure;

namespace DemoGame
{
    public class DemoGameInfo : GameInfo
    {
        public override string Name => "Demo Game";
        public override string Description => "The most exciting demo!";
        public override Version Version => new Version(1, 0, 0, 0);
        public override string Author => "Leisure.Net Contributors";
        public override string Prefix => "demo";
        public override string NumOfPlayers => "2";

        public override GameInstance CreateGame(uint id, ImmutableArray<IUser> players) => new DemoGameInstance(id, players);
        public override bool IsValidPlayerCount(uint i) => (i == 2);
    }
}