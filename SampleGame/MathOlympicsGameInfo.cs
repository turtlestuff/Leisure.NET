using System;
using System.Collections.Immutable;
using Discord;
using Leisure;

namespace MathOlympics
{
    public sealed class MathOlympicsGameInfo : GameInfo
    {
        public override string Name => "Math Olympics";
        public override string Description => "Compete against your friends to see who can answer math questions the fastest! Math Olympics will ask you and your friends 10 questions in the form `<number> <operator> <number> = ?`. Numbers can be between 1 and 100, and the operator can be +, -, ×, or ÷. The fastest to complete all 10 questions wins!";
        public override string Author => "Leisure.NET Contributors";
        public override string Prefix => "math";
        public override string IconUrl => "https://cdn.discordapp.com/avatars/614127017666936835/3290f301318a46c34e8a4d0671abeff4.png?size=2048";
        public override string PlayerCountDescription => "Any amount";
        public override GameInstance CreateGame(int id, IDiscordClient client, ImmutableArray<IUser> players, ImmutableArray<IUser> spectators) => new MathOlympicsGameInstance(id, client, players, spectators);
        public override bool IsValidPlayerCount(int i) => true;
        public override bool SupportsSpectators => true;
    }
}