using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_BOT
{
    public class ApexDropPoint
    {
        public async Task<Bitmap> CreateDropPoint()
        {
            return await Task.Run(async () =>
            {
                Bitmap map = new Bitmap("ApexImage/ApexMap.png");
                Bitmap drop = new Bitmap("ApexImage/Drop.png");

                Graphics g = Graphics.FromImage(map);

                Point p = await GetPoint();
                g.DrawImage(drop, p.X - (drop.Width/2), p.Y - (drop.Height/2), drop.Width, drop.Height);
                g.Dispose();

                return map;
            });
        }

        public async Task<Point> GetPoint()
        {
            return await Task.Run(() =>
            {
                Point[] point =
                {
                     new Point(290,200),//スラムレイク
                     new Point(357,275),//ピット
                     new Point(250,344),//ランオフ
                     new Point(250,490),//航空基地
                     new Point(240,390),//バンカー
                     new Point(546,293),//カスケーズ
                     new Point(610,136),//砲台
                     new Point(843,185),//リレー
                     new Point(818,272),//湿地
                     new Point(830,538),//ハイドロダム
                     new Point(940,435),//沼沢
                     new Point(839,485),//ブリッジ
                     new Point(571,588),//マーケット
                     new Point(626,790),//水処理施設
                     new Point(425,646),//スカルタウン
                     new Point(340,736),//サンダードーム
                     new Point(824,645),//理パルサー
                };
                 
                int seed = Environment.TickCount;
                Random r = new Random(seed);
                int result = r.Next(0,17);

                return point[result];
            });
        }
    }
}
