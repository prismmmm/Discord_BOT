using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_BOT
{
    public class Helper
    {
        public string ConvertPlatform(int platformId)
        {
            var platform = "";
            switch (platformId)
            {
                case 1:
                    platform = "XBOX";
                    break;
                case 2:
                    platform = "PSN";
                    break;
                case 5:
                    platform = "PC";
                    break;
                default:
                    platform = "not found platform";
                    break;
            }

            return platform;
        }

        public string ConvertTracker(string legend,string tracker)
        {
            string result = "";
            if (legend == "Wraith")
            {
                switch (tracker)
                {
                    case "Legend Specific 1":
                        result = "フェーズウォーク 発動時間";
                        break;
                    case "Legend Specific 2":
                        result = "次元断層 仲間のワープ回数";
                        break;
                    case "Legend Specific 3":
                        result = "呼び声 警告取得数";
                        break;
                    default:
                        result = tracker;
                        break;
                }
            }else if(legend == "Caustic")
            {
                switch (tracker)
                {
                    case "Legend Specific 1":
                        result = "NOXガスによるダメージ数";
                        break;
                    case "Legend Specific 2":
                        result = "ガストラップ作動回数";
                        break;
                    case "Legend Specific 3":
                        result = "NOXガス使用敵キル数";
                        break;
                    default:
                        result = tracker;
                        break;
                }
            }
            else
            {
                result = tracker;
            }
           

            return result;
        }

    }
}
