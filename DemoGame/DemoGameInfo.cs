using System;
using System.Collections.Generic;
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
        public override string PlayerCountDescription => "3 players only";

        public override GameInstance CreateGame(int id, HashSet<IUser> players) => new DemoGameInstance(id, players);
        public override bool IsValidPlayerCount(int i) => i == 3;
    }
}