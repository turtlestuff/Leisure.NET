using System.Collections.Generic;
using Discord;
using Leisure;

namespace DemoGame
{
    public class DemoGameInfo : GameInfo
    {
        public override string Name => "Demo Game";
        public override string Description => "The most exciting demo!";
        public override string Author => "Leisure.NET Contributors";
        public override string Prefix => "demo";
        public override string PlayerCountDescription => "3 players only";
        public override GameInstance CreateGame(int id, HashSet<IUser> players) => new DemoGameInstance(id, players);
        public override bool IsValidPlayerCount(int i) => i == 1;
    }
}