using System.Collections.Generic;
using Discord;

namespace Leisure.NET
{
    public abstract class Game
    {

        public List<IUser> Players { get; internal set; }
        public uint Id { get; internal set; }
        
        
        public abstract void Initialize();
        public abstract void ProcessCommand(IUserMessage message);

    }
}