using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GraphLibrary.Algorithm;
using GraphLibrary.Factory;
using GraphLibrary.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphUnitTests.A3
{
    [TestClass]
    public class TestA3
    {

        /// <summary>
        /// Informationen für einen Testlauf in dieser Aufgabe
        /// </summary>
        public class TestData
        {
            public string Name { get; set; }
            public string File { get; set; }
        }


        [TestMethod]
        public void A3()
        {
            var costName = "Kosten";
            List<TestData> testFiles = new List<TestData>();

            testFiles.Add(new TestData { Name = "K_10", File = @"A3\SampleData\K_10.txt" });
            testFiles.Add(new TestData { Name = "K_10e", File = @"A3\SampleData\K_10e.txt" });
            testFiles.Add(new TestData { Name = "K_12", File = @"A3\SampleData\K_12.txt" });
            testFiles.Add(new TestData { Name = "K_12e", File = @"A3\SampleData\K_12e.txt" });
            testFiles.Add(new TestData { Name = "K_15", File = @"A3\SampleData\K_15.txt" });
            testFiles.Add(new TestData { Name = "K_15e", File = @"A3\SampleData\K_15e.txt" });
            testFiles.Add(new TestData { Name = "K_20", File = @"A3\SampleData\K_20.txt" });
            testFiles.Add(new TestData { Name = "K_30", File = @"A3\SampleData\K_30.txt" });
            testFiles.Add(new TestData { Name = "K_50", File = @"A3\SampleData\K_50.txt" });
            testFiles.Add(new TestData { Name = "K_70", File = @"A3\SampleData\K_70.txt" });
            testFiles.Add(new TestData { Name = "K_100", File = @"A3\SampleData\K_100.txt" });




            foreach (var data in testFiles)
            {
                // Initialisieren

                // Einlesen in String 
                string s = File.ReadAllText(data.File);
                IGraph g = GraphFactory.GraphFromAdjListStringWithCost(s, data.Name, costName, false);

                // Rechnen
                var k = TSP.NearestNeighbour(g, costName);

                Assert.IsNotNull(k, "NearestNeighbour Null");
                var costs = k.Sum(x => x.Costs[costName]);
                Assert.IsTrue(costs > 0, "NearestNeighbour Sum");


                k = TSP.DoubleTree(g, costName);
                Assert.IsNotNull(k, "DoubleTree Null");
                costs = k.Sum(x => x.Costs[costName]);
                Assert.IsTrue(costs > 0, "DoubleTree Sum");
            }


            string s1 = File.ReadAllText(testFiles[0].File);
            var g1 = GraphFactory.GraphFromAdjListStringWithCost(s1, testFiles[0].Name, costName, false);
            var k1 = TSP.TryAllTours(g1, costName, false);
            Assert.AreEqual(38.41d, k1.Sum(x => x.Costs[costName]), "Alle Touren");

            var k2 = TSP.TryAllTours(g1, costName, true);
            Assert.AreEqual(38.41d, k2.Sum(x => x.Costs[costName]), "Alle Touren B&B");
        }
    }
}
