using System;
using System.Collections.Immutable;
using Discord;
using Leisure;

namespace DemoGame
{
    public class DemoGameInfo : GameInfo
    {
        public override String Name => "Demo Game";
        public override String Description => "The most exciting demo!";
        public override Version Version => new Version(1, 0, 0, 0);
        public override String Author => "Leisure.Net Contributors";
        public override String Prefix => "demo";
        public override Int32 MaxPlayers => 2;

        public override GameInstance CreateGame(uint id, ImmutableArray<IUser> players) => new DemoGameInstance(id, players);
    }
}