using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Leisure;
using Microsoft.Extensions.DependencyInjection;

namespace DemoGame
{
    public class DemoGameInstance : GameInstance
    {
        public CommandService Commands { get; set; } = default!;

        public IServiceProvider ServiceProvider { get; set; } = default!;
        
        public DemoGameInstance(IDiscordClient client, HashSet<IUser> players, int id) : base(client, players, id)
        {
        }

        public override async Task Initialize()
        {
            ServiceProvider = new ServiceCollection().AddSingleton(this).BuildServiceProvider();
            Commands = new CommandService();
            await Commands.AddModuleAsync<DemoGameModule>(ServiceProvider);
            await Broadcast("Ready game #" + Id);
        }
        
        public override async Task OnMessage(IUserMessage msg, int pos)
        {
            var result = await Commands.ExecuteAsync(new CommandContext(Client, msg), pos, ServiceProvider);

            if (!result.IsSuccess && result.Error == CommandError.UnknownCommand)
            {
                await BroadcastExcluding($"*{Id}* {msg.Author.Username}: {msg.Content}", exclude: msg.Author);
                await msg.AddReactionAsync(new Emoji("✅"));
            }
        }

        internal void Close()
        {
            OnClosing();
        }
    }
}