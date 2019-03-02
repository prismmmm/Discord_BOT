using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Discord_BOT
{
    public class Apex
    {
        public async Task<Player> Stats(string name)
        {
            return await Task.Run(() =>
             {
                 Dictionary<int, string> legends = new Dictionary<int, string>()
                 {
                    {1,"a" },
                    {2,"a" },
                    {3,"a" },
                    {4,"a" },
                    {5,"a" },
                    {6,"a" },
                    {7,"a" },
                    { 8,"a" }
                 };

                 //nuget Microsoft.Json
                 //参照 System.Net.Http
                 string baseUrl = $"https://public-api.tracker.gg/apex/v1/standard/profile/5/{name}";
                 //東京都のID
                 string cityname = "130010";

                 string url = $"{baseUrl}?city={cityname}";

                 HttpClient client = new HttpClient();
                 client.DefaultRequestHeaders.Add("TRN-Api-Key", $"e751dadd-3eca-4e4b-b10a-05c6ce651a51");
                 string json = client.GetStringAsync(url).Result;

                 JObject jobj = JObject.Parse(json);


                 var d = new Data2();

                 Data2[] datas = new Data2[0];
                 Player resultP = new Player();
                 resultP.PlayerName = (string)((jobj["data"]["metadata"]["platformUserHandle"] as JValue).Value);
                 resultP.Level = (string)((jobj["data"]["metadata"]["level"] as JValue).Value).ToString();
                 resultP.PlatFormId = int.Parse(((jobj["data"]["metadata"]["platformId"] as JValue).Value).ToString());
                 resultP.PlatFormId = int.Parse(((jobj["data"]["metadata"]["platformId"] as JValue).Value).ToString());
                 resultP.Thumbnail = (string)((jobj["data"]["children"][0]["metadata"]["icon"] as JValue).Value);

                 int hitCount = 0;
                 for (int i = 0; i < 8; i++)
                 {
                     try { var test = (string)((jobj["data"]["children"][i]["metadata"]["legend_name"] as JValue).Value); }
                     catch { continue; }

                     Array.Resize(ref datas, datas.Length + 1);
                     datas[datas.Length - 1] = new Data2();
                     string[] keys = new string[0];
                     string[] values = new string[0];

                     try
                     {
                         datas[datas.Length - 1].LegendName = (string)((jobj["data"]["children"][i]["metadata"]["legend_name"] as JValue).Value);
                     }
                     catch
                     {
                         datas[datas.Length - 1].LegendName = "no";
                     }
                   

                     var count = 0;
                     while (true)
                     {
                         try
                         {
                             var test = (string)((jobj["data"]["children"][i]["stats"][count]["metadata"]["key"] as JValue).Value).ToString();
                         }
                         catch
                         {
                             break;
                         }

                         Array.Resize(ref keys, keys.Length + 1);
                         Array.Resize(ref values, values.Length + 1);
                         datas[datas.Length - 1].KillRank = (string)((jobj["data"]["children"][i]["stats"][count]["metadata"]["key"] as JValue).Value).ToString();
                         keys[keys.Length - 1] = (string)((jobj["data"]["children"][i]["stats"][count]["metadata"]["key"] as JValue).Value).ToString();
                         values[values.Length - 1] = (string)((jobj["data"]["children"][i]["stats"][count]["displayValue"] as JValue).Value).ToString();
                         count++;
                     }

                     datas[hitCount].Key = keys;
                     datas[hitCount].Value = values;

                     hitCount++;
                 }

                 string platform = "";
                 switch (resultP.PlatFormId)
                 {
                     case 5:
                         platform = "PC";
                         break;
                 }

                 resultP.Datas = ConvertKey(datas);

                 Console.WriteLine("result ^^^^^^^^^^^^^^^^^^^^^^^^");
                 foreach (var a in resultP.Datas)
                 {
                     Console.WriteLine(a.LegendName);
                     for (int j = 0; j < a.Key.Length; j++)
                     {
                         Console.WriteLine($"{a.Key[j]}:{a.Value[j]}");
                     }
                 }

                 return resultP;
             });

        }


        Data2[] ConvertKey(Data2[] datas)
        {

            for (int l = 0; l < datas.Length; l++)
            {
                if (datas[l].LegendName == "Caustic")
                {
                    for (int i = 0; i < datas[l].Key.Length; i++)
                    {
                        var res = "";
                        switch (datas[l].Key[i])
                        {
                            case "Specific1":
                                res = "NOX ガスダメージ数";
                                break;
                            default:
                                res = datas[l].Key[i];
                                break;
                        }

                        datas[l].Key[i] = res;
                    }

                }
            }


            return datas;
        }
    }


    public class Data2
    {
        public string LegendName { get; set; }
        public string KillRank { get; set; }
        public string[] Key { get; set; }
        public string[] Value { get; set; }
    }

    public class Player
    {
        public string PlayerName { get; set; }
        public int PlatFormId { get; set; }
        public string Level { get; set; }
        public Data2[] Datas { get; set; }
        public string Thumbnail { get; set; }
    }
}

