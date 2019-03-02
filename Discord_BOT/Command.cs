
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
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
        [Command("t", RunMode = RunMode.Async)]
        public async Task T()
        {
            await Context.Channel.SendMessageAsync("aa");
        }

        [Command("a", RunMode = RunMode.Async)]
        public async Task GetApexStats(string name)
        {
            Helper helper = new Helper();
            Apex2 a = new Apex2();
            Player2 p = await a.Stats(name);


            EmbedBuilder eb = new EmbedBuilder();
            eb.WithColor(new Color(0, 255, 0))
            .WithTitle($"**{p.PlayerName}**")
            .WithDescription($@"  Platform: **{p.Platform}**")
            .WithFooter(f =>f.Text = "data from: apex.tracker.gg | Bot created by Prism")
            .WithCurrentTimestamp()
            .WithColor(new Color(254,100,46))
            //.WithUrl(anime.Url)
            .WithThumbnailUrl(p.LegendThumbnail);
            //.WithAuthor(x =>
            //{
            //    x.Name = author.Nickname == null ? author.Username : author.Nickname;
            //    x.IconUrl = author.AvatarUrl;
            //});

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
            //await Context.Channel.SendMessageAsync("aa");
        }


    }
}
