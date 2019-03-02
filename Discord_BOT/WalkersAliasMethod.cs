using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_BOT
{
    public class WalkersAliasMethod
    {
        private double[] probList;
        private int[] aliasList;
        private double[] weightList;
        private Random rnd;

        public WalkersAliasMethod()
        {
            rnd = new Random();
        }

        public WalkersAliasMethod(int seed)
        {
            rnd = new Random(seed);
        }


        public void UpdateList(double[] weightList)
        {
            probList = new double[weightList.Length];
            aliasList = new int[weightList.Length];
            this.weightList = weightList;
            int size = weightList.Length;
            double[] norWeightList = new double[size];
            weightList.CopyTo(norWeightList, 0);
            double sum = weightList.Sum();
            double[] v = new double[size];
            for (int i = 0; i < size; i++)
            {
                norWeightList[i] /= sum;
                v[i] = norWeightList[i] * size;
            }

            List<int> small = new List<int>();
            List<int> large = new List<int>();

            for (int i = 0; i < size; i++)
            {

                if (v[i] < 1)
                    small.Add(i);
                else
                    large.Add(i);
            }

            int g, l;
            while (small.Count > 0 && large.Count > 0)
            {
                l = small[0];
                g = large[0];
                small.RemoveAt(0);
                large.RemoveAt(0);

                probList[l] = v[l];
                aliasList[l] = g;
                v[g] += -1.0 + v[l];
                if (v[g] < 1)
                    small.Add(g);
                else
                    large.Add(g);
            }
            while (large.Count > 0)
            {
                g = large[0];
                large.RemoveAt(0);
                probList[g] = 1;
            }
            while (small.Count > 0)
            {
                l = small[0];
                small.RemoveAt(0);
                probList[l] = 1;
            }
        }

        public int Resampling()
        {
            double v = rnd.NextDouble() * (double)weightList.Length;
            int k = (int)v;
            double u = 1 + k - v;
            if (u < probList[k])
            {
                return k;
            }
            return aliasList[k];
        }
    }
}
