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
        
        public MathOlympicsGameInstance(IDiscordClient client, ImmutableArray<IUser> players, int id) : base(client, players, id)
        {
            UserCompletion = new ConcurrentDictionary<IUser, UserState>(
                players.Select(p => new KeyValuePair<IUser, UserState>(p, new UserState())), DiscordComparers.UserComparer);
        }
        
        const int MaxExpressions = 10;

        int DoneUsers;

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
            await Broadcast($"Game {Id} has started. {Expressions[0]} = ?");
        }


        async Task SendResults()
        {
            var sort = UserCompletion.OrderByDescending(k => k.Value.FinishTime);

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
                    }} {key.Mention} - {((value.FinishTime - StartTime)?.TotalSeconds is double s ? s.ToString("F1") : "Dropped")}s";
                }))
            };

            await Broadcast("The results are in!", embed: builder.Build());
            Close();
        }
        
        public override async Task OnMessage(IUserMessage msg, int pos)
        {
            if (msg.Content.AsSpan(pos).Equals("drop", default))
            {
                await DropPlayer(msg.Author);
                return;
            }
            
            if (double.TryParse(msg.Content, out var num))
            {
                if (Math.Abs(Expressions[UserCompletion[msg.Author].Progress].Compute() - num) < 0.01)
                {
                    if (++UserCompletion[msg.Author].Progress is var index && index == MaxExpressions)
                    {
                        var time = UserCompletion[msg.Author].FinishTime = DateTime.UtcNow;
                        await msg.Author.SendMessageAsync(text: $"That was correct! You finished in {(time - StartTime)?.TotalSeconds:F1}s. Waiting for other players to finish...");

                        if (Interlocked.Add(ref DoneUsers, 1) == Players.Length)
                        {
                            await SendResults();
                        }
                    }
                    else
                    {
                        await msg.Author.SendMessageAsync($"That was correct! {Expressions[index]} = ?");
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
            OnDroppingPlayer(player);
            UserCompletion.Remove(player, out _);
            if (DoneUsers == Players.Length)
            {
                await SendResults();
            }
        }
    }
}