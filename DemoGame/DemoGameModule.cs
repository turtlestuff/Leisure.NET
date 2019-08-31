using System.Threading.Tasks;
using Discord.Commands;

namespace DemoGame
{
    public class DemoGameModule : ModuleBase
    {
        public DemoGameInstance Game { get; set; } = default!;
        
        [Command("close")]
        public async Task Close()
        {
            await Game.Broadcast("Closing game " + Game.Id.ToString());
            Game.Close();
        }
    }
}