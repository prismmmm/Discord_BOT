using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

namespace Discord_BOT
{
    class Apex2
    {
        public async Task<Player2> Stats(string name)
        {
            return await Task.Run(() =>
            {
                var helper = new Helper();

                string baseUrl = $"https://public-api.tracker.gg/apex/v1/standard/profile/5/{name}";
                string url = $"{baseUrl}";

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("TRN-Api-Key", $"e751dadd-3eca-4e4b-b10a-05c6ce651a51");
                string json = client.GetStringAsync(url).Result;

                Rootobject pDeserializeList = JsonConvert.DeserializeObject<Rootobject>(json);

                var player = new Player2();
                var _legends = new Legends[0];


                player.PlayerName = pDeserializeList.data.metadata.platformUserHandle;
                player.Platform = helper.ConvertPlatform(pDeserializeList.data.metadata.platformId);
                player.PlayerLevel = pDeserializeList.data.metadata.level;
                player.TotalKill = (int)pDeserializeList.data.stats[1].value;

                foreach (var child in pDeserializeList.data.children)
                {
 
                    Array.Resize(ref _legends, _legends.Length + 1);
                    _legends[_legends.Length - 1] = new Legends();
                    _legends[_legends.Length - 1].data = new Dictionary<string, string>();

                    _legends[_legends.Length - 1].LegendName = child.metadata.legend_name;

                    //最初のレジェンドの画像のみ取得
                    if (player.LegendThumbnail == null)
                        player.LegendThumbnail = child.metadata.icon;

                    foreach (var s in child.stats)
                    {
                        _legends[_legends.Length - 1].data.Add(helper.ConvertTracker(child.metadata.legend_name, s.metadata.name), s.displayValue);
                        if (s.metadata.name == "Kills")
                            _legends[_legends.Length - 1].KillRank = s.displayRank;
                    }
                }

                player.legends = _legends;


                Console.WriteLine($"Player Name : {player.PlayerName}");
                Console.WriteLine($"PlatForm : {player.Platform}");
                Console.WriteLine($"Level : {player.PlayerLevel}");
                foreach (var p in player.legends)
                {
                    foreach (KeyValuePair<string, string> pair in p.data)
                    {
                        Console.WriteLine($"{pair.Key} : {pair.Value}");
                    }
                    Console.WriteLine($"Kill rank : {p.KillRank}\r\n");
                }

                return player;
            });
        }

        public static class JsonUtility
        {
            /// <summary>
            /// Jsonメッセージをオブジェクトへデシリアライズします。
            /// </summary>
            public static T Deserialize<T>(string message)
            {
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(message)))
                {
                    var setting = new DataContractJsonSerializerSettings()
                    {
                        UseSimpleDictionaryFormat = true,
                    };
                    var serializer = new DataContractJsonSerializer(typeof(T), setting);
                    return (T)serializer.ReadObject(stream);
                }
            }
        }
    }




    public class Legends
    {
        public Dictionary<string, string> data { get; set; }
        public string KillRank { get; set; }
        public string LegendName { get; set; }
    }


    public class Player2
    {
        public string PlayerName { get; set; }
        public string Platform { get; set; }
        public string LegendThumbnail { get; set; }
        public int PlayerLevel { get; set; }
        public int TotalKill { get; set; }
        public Legends[] legends { get; set; }
    }

    public class Rootobject
    {
        public Data data { get; set; }
    }

    public class Data
    {
        public string id { get; set; }
        public string type { get; set; }
        public Child[] children { get; set; }
        public Metadata metadata { get; set; }
        public Stat1[] stats { get; set; }
    }

    public class Metadata
    {
        public string[] statsCategoryOrder { get; set; }
        public int platformId { get; set; }
        public string platformUserHandle { get; set; }
        public string accountId { get; set; }
        public string cacheExpireDate { get; set; }
        public int level { get; set; }
    }

    public class Child
    {
        public string id { get; set; }
        public string type { get; set; }
        public Metadata1 metadata { get; set; }
        public Stat[] stats { get; set; }
    }

    public class Metadata1
    {
        public string legend_name { get; set; }
        public string icon { get; set; }
        public string bgimage { get; set; }
    }

    public class Stat
    {
        public Metadata2 metadata { get; set; }
        public float value { get; set; }
        public float percentile { get; set; }
        public string displayValue { get; set; }
        public string displayRank { get; set; }
    }

    public class Metadata2
    {
        public string key { get; set; }
        public string name { get; set; }
        public string categoryKey { get; set; }
        public string categoryName { get; set; }
        public bool isReversed { get; set; }
    }

    public class Stat1
    {
        public Metadata3 metadata { get; set; }
        public float value { get; set; }
        public float percentile { get; set; }
        public string displayValue { get; set; }
        public string displayRank { get; set; }
    }

    public class Metadata3
    {
        public string key { get; set; }
        public string name { get; set; }
        public string categoryKey { get; set; }
        public string categoryName { get; set; }
        public bool isReversed { get; set; }
    }

}
