using GraphLibrary.Algorithm;
using GraphLibrary.Factory;
using GraphLibrary.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A3
{
    class Program
    {
        static void Main(string[] args)
        {

            var costName = "Kosten";


            var s = File.ReadAllText(@"SampleData\K_10.txt");

            IGraph gTest = GraphFactory.GraphFromAdjListStringWithCost(s, "T1", costName, false);


            var sw = System.Diagnostics.Stopwatch.StartNew();
            var bestWay = TSP.TryAllTours(gTest, costName, true);


            //var bestWay = TSP.NearestNeighbour(gTest, costName);

            //var bestWay = TSP.DoubleTree(gTest, costName);


            #region Test

            //long cnt = 0;

            //int n = 10;
            //for (int i1 = 0; i1 < n; ++i1)
            //    for (int i2 = 0; i2 < n - 1; ++i2)
            //        for (int i3 = 0; i3 < n - 2; ++i3)
            //            for (int i4 = 0; i4 < n - 3; ++i4)
            //                for (int i5 = 0; i5 < n - 4; ++i5)
            //                    for (int i6 = 0; i6 < n - 5; ++i6)
            //                        for (int i7 = 0; i7 < n - 6; ++i7)
            //                            for (int i8 = 0; i8 < n - 7; ++i8)
            //                                for (int i9 = 0; i9 < n - 8; ++i9)
            //                                    for (int i10 = 0; i10 < n - 9; ++i10)
            //                                        ++cnt;


            //int n = 12;
            //for (int i1 = 0; i1 < n; ++i1)
            //    for (int i2 = 0; i2 < n - 1; ++i2)
            //        for (int i3 = 0; i3 < n - 2; ++i3)
            //            for (int i4 = 0; i4 < n - 3; ++i4)
            //                for (int i5 = 0; i5 < n - 4; ++i5)
            //                    for (int i6 = 0; i6 < n - 5; ++i6)
            //                        for (int i7 = 0; i7 < n - 6; ++i7)
            //                            for (int i8 = 0; i8 < n - 7; ++i8)
            //                                for (int i9 = 0; i9 < n - 8; ++i9)
            //                                    for (int i10 = 0; i10 < n - 9; ++i10)
            //                                        for (int i11 = 0; i11 < n - 10; ++i11)
            //                                            for (int i12 = 0; i12 < n - 11; ++i12)
            //                                                ++cnt;


            //Test(12);


            //List<List<int>> all = new List<List<int>>();
            //int[] elems = Enumerable.Repeat(0, 12).ToArray();
            //Perm(elems, 0, all);

            #endregion Test


            sw.Stop();


            foreach (var elem in bestWay)
            {
                Console.WriteLine(elem.Identifier);
            }

            Console.WriteLine(bestWay.Sum(x => x.Costs[costName]));
            Console.WriteLine($"{sw.ElapsedMilliseconds} ms");

            Console.ReadLine();
        }




        #region Test


        static int cnt = 0;

        private static void Test(int n)
        {
            for (int i = 0; i < n; i++)
            {
                TestRecursion(n - 1);
            }
        }


        private static void TestRecursion(int n)
        {
            if (n == 0)
            {
                ++cnt;
            }

            for (int i = 0; i < n; i++)
            {
                TestRecursion(n - 1);
            }
        }




        static void Perm<T>(T[] arr, int k, List<List<T>> all)
        {
            if (k >= arr.Length)
            {
                //all.Add(arr.ToList());
                ++cnt;
            }
            else
            {
                Perm(arr, k + 1, all);
                for (int i = k + 1; i < arr.Length; i++)
                {
                    Swap(ref arr[k], ref arr[i]);
                    Perm(arr, k + 1, all);
                    Swap(ref arr[k], ref arr[i]);
                }
            }
        }

        private static void Swap<T>(ref T item1, ref T item2)
        {
            T temp = item1;
            item1 = item2;
            item2 = temp;
        }



        #endregion Test
    }
}
