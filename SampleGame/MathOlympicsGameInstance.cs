using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Leisure;

namespace MathOlympics
{
    public sealed class MathOlympicsGameInstance : GameInstance
    {
        class UserState
        {
            public int Progress { get; set; }
            public DateTime? FinishTime { get; set; }
        }
        
        public List<Expression> Expressions { get; set; } = default!;
        
        public DateTime StartTime { get; set; }

        ConcurrentDictionary<IUser, UserState> UserCompletion { get; }
        
        public MathOlympicsGameInstance(int id, IDiscordClient client, ImmutableArray<IUser> players, ImmutableArray<IUser> spectators) 
            : base(id, client, players, spectators)
        {
            UserCompletion = new ConcurrentDictionary<IUser, UserState>(
                players.Select(p => new KeyValuePair<IUser, UserState>(p, new UserState())), DiscordComparers.UserComparer);

            OriginalPlayerCount = players.Length;
        }
        
        const int MaxExpressions = 1;

        int DoneUsers;

        int OriginalPlayerCount;

        public override async Task Initialize()
        {
            
            var expressions = new List<Expression>();
            
            var random = new Random();
            for (var i = 0; i < MaxExpressions; i++)
            {
                var left = random.Next(0, 100) + 1;
                var right = random.Next(0, 100) + 1;
                var op = random.Next(0, 4);
                
                expressions.Add(new Expression { FirstOperand = left, SecondOperand = right, Operator = (Operator) op });
            }
            
            Expressions = expressions;

            StartTime = DateTime.UtcNow;
            await this.BroadcastTo($"Game {Id} has started. {Expressions[0]} = ?", players: Players);
            await this.BroadcastTo($"Game {Id} has started. Waiting for competition to end...", players: Spectators);
        }

        Task IncrementAndTest() => Interlocked.Add(ref DoneUsers, 1) == OriginalPlayerCount ? SendResults() : Task.CompletedTask;

        async Task SendResults()
        {
            var sort = UserCompletion.OrderBy(k => k.Value.FinishTime ?? DateTime.MaxValue);

            var builder = new EmbedBuilder
            {
                Title = "Results",
                Description = string.Join("\n", sort.Select((k, i) =>
                {
                    var (key, value) = k;
                    return $@"{i switch
                    {
                        0 => "**🥇 First Place**",
                        1 => "**🥈 Second Place**",
                        2 => "**🥉 Third Place**",
                        _ => $"#{i + 1}"
                    }} {key.Mention} - {((value.FinishTime - StartTime)?.TotalSeconds is double s ? $"{s:F1}s" : "Dropped")}";
                }))
            };

            await this.Broadcast("The results are in!", embed: builder.Build());
            Close();
        }
        
        public override async Task OnMessage(IUserMessage msg, int pos)
        {
            if (msg.Content.AsSpan(pos).Equals("drop", default))
            {
                await DropPlayer(msg.Author);
                return;
            }
            
            // Do not react to spectator messages
            if (!UserCompletion.ContainsKey(msg.Author)) 
                return;

            // Don't try to do anything if the user is done
            if (UserCompletion[msg.Author].Progress is MaxExpressions) return;

            if (double.TryParse(msg.Content, out var num))
            {
                if (Math.Abs(Expressions[UserCompletion[msg.Author].Progress].Compute() - num) < 0.01)
                {
                    switch (++UserCompletion[msg.Author].Progress)
                    {
                        case MaxExpressions:
                            var time = UserCompletion[msg.Author].FinishTime = DateTime.UtcNow;
                            await msg.Author.SendMessageAsync(
                                $"That was correct! You finished in {(time - StartTime)?.TotalSeconds:F1}s. Waiting for other players to finish...");

                            await IncrementAndTest();
                            break;
                        case var index:
                            await msg.Author.SendMessageAsync($"That was correct! {Expressions[index]} = ?");
                            break;
                    }
                }
                else
                {
                    await msg.Author.SendMessageAsync($"Your answer was not correct. {Expressions[UserCompletion[msg.Author].Progress]} = ?");
                }
            }
            else
            {
                await msg.Author.SendMessageAsync($"That is not a valid answer. {Expressions[UserCompletion[msg.Author].Progress]} = ?");
            }
        }

        void Close()
        {
            OnClosing();
        }
        
        async Task DropPlayer(IUser player)
        {
            OnDroppingUser(player);
            await IncrementAndTest();
        }
    }
}