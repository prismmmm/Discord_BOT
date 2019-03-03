
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Discord_BOT
{
    public class Command : ModuleBase<ICommandContext>
    {
        public async Task Delete(IUserMessage message)
        {
            await Task.Run(async () =>
             {
                 await Task.Delay(30000);
                 await message.DeleteAsync();
             });
        }

        [Command("stats", RunMode = RunMode.Async)]
        [Alias("s")]
        public async Task GetApexStats(string name)
        {
            Helper helper = new Helper();
            ApexStats a = new ApexStats();
            Player p = await a.Stats(name);
            if(p == null)
            {
                await Context.Channel.SendMessageAsync("");
                return;
            }

            EmbedBuilder eb = new EmbedBuilder();
            eb.WithColor(new Discord.Color(0, 255, 0))
            .WithTitle($"**{p.PlayerName}**")
            .WithDescription($@"  Platform: **{p.Platform}**")
            .WithFooter(f =>f.Text = "data from: apex.tracker.gg | Bot created by Prism")
            .WithCurrentTimestamp()
            .WithColor(new Discord.Color(254,100,46))
            .WithThumbnailUrl(p.LegendThumbnail);

            eb.AddField(x =>
            {
                x.IsInline =false;
                x.Name = $"General";
                x.Value += "\n";
                x.Value += $"  Level : {p.PlayerLevel} \n";
                x.Value += $"  Kills : {p.TotalKill} \n";
                x.Value += "";
            });

            foreach (var legend in p.legends)
            {

                eb.AddField(x =>
                {
                    x.IsInline = true;
                    x.Name = $"■{legend.LegendName}";
                    x.Value += $"```cpp\n";
                    x.Value += $"Kill Rank : #{legend.KillRank} \n";
                    foreach (KeyValuePair<string, string> pair in legend.data)
                    {
                        x.Value += $"{pair.Key} : {pair.Value}\n";
                    }

                    x.Value += "```";
                });
            }


            await Context.Channel.SendMessageAsync("", embed: eb.Build());
            await Context.Message.DeleteAsync();
        }

        [Command("map", RunMode = RunMode.Async)]
        [Alias("drop","m","d")]
        public async Task DropPoint()
        {
            var filename = "ApexImage/result.png";
            ApexDropPoint adp = new ApexDropPoint();
            Bitmap map = await adp.CreateDropPoint();
            map.Save(filename);
            var map_message = await Context.Channel.SendFileAsync(filename);
            await Delete(map_message);
            var message = Context.Message;
            await message.DeleteAsync();
        }

        [Command("help", RunMode = RunMode.Async)]
        [Alias("h")]
        public async Task Help()
        {
            var userid = Context.User.Id;
            var user = await Context.Channel.GetUserAsync((ulong)userid);
            var dm = await user.GetOrCreateDMChannelAsync();

            EmbedBuilder eb = new EmbedBuilder();
            eb.WithColor(new Discord.Color(0, 255, 0))
            .WithTitle("HELP")
            .WithThumbnailUrl("https://data4.origin.com/asset/content/dam/originx/web/app/games/apex/apex/F2P/APEX_gdp_logo_ww_v2.png/faf6a423-e156-48af-b71e-2702fac826a0/original.png");

            eb.AddField(x => {
                x.IsInline = true;
                x.Name = "Command list";
                x.Value = "**stats**\n  `:stats {Origin_name}`\n PC only";
                x.Value += "**map**\n  `:map` `:m` `:drop` `:d` \n";
            });

            await dm.SendMessageAsync("", embed: eb.Build());
            await Context.Message.DeleteAsync();
        }
    }
}
