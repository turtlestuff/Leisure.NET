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
        public override string PlayerCountDescription => "Any amount";
        public override GameInstance CreateGame(IDiscordClient client, HashSet<IUser> players, int id) => new DemoGameInstance(client, players, id);
        public override bool IsValidPlayerCount(int i) => true;
    }
}