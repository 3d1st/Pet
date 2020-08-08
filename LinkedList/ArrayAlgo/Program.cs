using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;

namespace ArrayAlgo
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] source = {1, 0, 2, 3,7,6,5};

            Console.WriteLine("GetMissedViaBitArray: {0}",      GetMissedViaBitArray(source));
            foreach (var tuple in GetMissedViaMathAlgorithm(source))
            {
                Console.WriteLine("GetMissedViaMathAlgorithm: {0}", tuple);                
            }
            
        }

        private static (int x, int y) GetMissedViaBitArray(int[] source)
        {
            int? x = null;

            BitArray mask = new BitArray(source.Length + 2);

            foreach (int t in source)
            {
                mask.Set(t, true);
            }

            for (int i = 0; i < mask.Length; i++)
            {
                if (mask.Get(i)) continue;
                if (x == null)
                {
                    x = i;
                }
                else
                {
                    return (x.Value, i);
                }
            }

            Debug.Assert(x != null, nameof(x) + " != null");
            return ((int) x, source.Length);
        }

        private static List<(int x, int y)> GetMissedViaMathAlgorithm(int[] source)
        {
            int n = source.Length + 1;
            int k = n - 1;
            int nElementsSum = (k + 1) * (k + 2) / 2;
            int sourceSum = 0;

            int xorN = 0;
            int xorS = 0;

            for (int i = 0; i < source.Length; i++)
            {
                int el = source[i];

                sourceSum += el;
                xorS ^= el;
                xorN ^= i;
            }

            xorN ^= n;

            int subtraction = nElementsSum - sourceSum;

            var res = new List<(int x, int y)>();
            for (int i = 0; i <= n - 1; i++)
            {
                for (int j = i + 1; j <= n; j++)
                {
                    if (i != j)
                    {
                        if (i + j == subtraction)
                        {
                            res.Add((i, j));
                        }
                    }
                }
            }

            return res;
        }
    }
}