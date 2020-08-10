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
            int[] source = {1, 4, 2, 7, 8, 6, 5};

            Console.WriteLine("GetMissedViaIntMask: {0}", GetMissedViaIntMask(source));
            Console.WriteLine("GetMissedViaBitArray: {0}", GetMissedViaBitArray(source));
            Console.WriteLine("GetMissedViaMathAlgorithm: {0}", GetMissedViaMathAlgorithm(source));
        }

        private static (int x, int y) GetMissedViaIntMask(int[] source)
        {
            int mask = 0;
            int? x = null;

            foreach (int t in source)
            {
                int bitMask = 1 << t;
                mask |= bitMask;
            }

            int sourceLength = source.Length + 1;
            
            for (int i = 0; i < sourceLength; i++)
            {
                int tempMask = 1 << i;

                if ((mask & tempMask) == 0)
                {
                    if (x == null)
                    {
                        x = i;
                    }
                    else
                    {
                        return ((int) x, i);
                    }
                }
            }
            
            Debug.Assert(x != null, nameof(x) + " != null");
            return ((int) x, sourceLength);
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

        private static (int x, int y) GetMissedViaMathAlgorithm(int[] source)
        {
            int n = source.Length + 1;
            int nElementsSum = n * (n + 1) / 2;
            long nElementSumOfPower = n * (n + 1) * (2 * n + 1) / 6;
            int sourceSum = 0;
            long sourceSumOfPower = 0;

            foreach (int el in source)
            {
                sourceSum += el;
                sourceSumOfPower +=  (long)el * el;
            }

            int subtractionOfSums = nElementsSum - sourceSum;
            long subtractionOfSumsOfPower = nElementSumOfPower - sourceSumOfPower;

            for (int i = 0; i <= n - 1; i++)
            {
                for (int j = i + 1; j <= n; j++)
                {
                    bool matched =
                        i != j &&
                        i + j == subtractionOfSums &&
                        (long) i * i + (long) j * j == subtractionOfSumsOfPower;
                    if (matched)
                    {
                        return (i, j);
                    }
                }
            }
            throw new InvalidOperationException();
        }
    }
}