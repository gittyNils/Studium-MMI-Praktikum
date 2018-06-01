using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GraphLibrary.Algorithm;
using GraphLibrary.Factory;
using GraphLibrary.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphUnitTests.A2
{
    [TestClass]
    public class TestA2
    {


        /// <summary>
        /// Informationen für einen Testlauf in dieser Aufgabe
        /// </summary>
        public class TestData
        {
            public string Name { get; set; }
            public string File { get; set; }
            public double Result { get; set; }
        }


        [TestMethod]
        public void A2()
        {
            var costName = "Kosten";
            List<TestData> testFiles = new List<TestData>();

            testFiles.Add(new TestData { Name = "G_1_2", File = @"A2\SampleData\G_1_2.txt", Result = 286.7112d });
            testFiles.Add(new TestData { Name = "G_1_20", File = @"A2\SampleData\G_1_20.txt", Result = 29.5493d });
            testFiles.Add(new TestData { Name = "G_1_200", File = @"A2\SampleData\G_1_200.txt", Result = 3.0228d });
            testFiles.Add(new TestData { Name = "G_10_20", File = @"A2\SampleData\G_10_20.txt", Result = 2775.4412d });
            //testFiles.Add(new TestData { Name = "G_10_200", File = @"A2\SampleData\G_10_200.txt", Result = 301.5519d });
            //testFiles.Add(new TestData { Name = "G_100_200", File = @"A2\SampleData\G_100_200.txt", Result = 27450.6171d });



            foreach (var data in testFiles)
            {
                // Initialisieren

                // Einlesen in String 
                string s = File.ReadAllText(data.File);
                IGraph g = GraphFactory.GraphFromAdjListStringWithCost(s, data.Name, costName, false);

                // Rechnen
                var k = MST.Kruskal(g, costName);

                var costs = k.Edges.Values.Sum(x => x.Values[costName]);
                Assert.AreEqual(Math.Round(data.Result, 4), Math.Round(costs, 4), nameof(MST.Kruskal));
                
                k = MST.KruskalV1(g, costName);
                Assert.AreEqual(Math.Round(data.Result, 4), Math.Round(costs, 4), nameof(MST.KruskalV1));
                

                var p = MST.Prim(g, costName);
                Assert.AreEqual(Math.Round(data.Result, 4), Math.Round(costs, 4), nameof(MST.Prim));

            }
        }
    }
}
