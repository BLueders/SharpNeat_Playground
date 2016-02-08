using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeatSine
{
    public static class ThreadSafeRandom
    {
        private static readonly Random Global = new Random();
        [ThreadStatic]
        private static Random _local;

        public static int Next()
        {
            Random inst = _local;
            if (inst == null)
            {
                int seed;
                lock (Global) seed = Global.Next();
                _local = inst = new Random(seed);
            }
            return inst.Next();
        }

        public static double NextDouble()
        {
            Random inst = _local;
            if (inst == null)
            {
                int seed;
                lock (Global) seed = Global.Next();
                _local = inst = new Random(seed);
            }
            return inst.NextDouble();
        }
    }
}
