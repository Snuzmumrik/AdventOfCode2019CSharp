using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day6._1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Planet> planets = new List<Planet>();
            using (var sr = File.OpenText("./input.txt"))
            {
                var line = "";
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    Planet newPlanet = new Planet();
                    newPlanet.Name = line.Substring(line.IndexOf(')') + 1);
                    newPlanet.OrbitsPlaceholder = line.Substring(0, line.IndexOf(')'));
                    planets.Add(newPlanet);

                }

                Planet COM = new Planet();
                COM.IsRoot = true;
                COM.Name = "COM";
                COM.OrbitTotal = 0;
                planets.Add(COM);
            }

            MapOrbits(planets);
            CountOrbits(planets);

            

            var checksum = 0;
            for (var i = 0; i < planets.Count; i++)
            {
                checksum += planets[i].OrbitTotal;
            }
            Console.WriteLine("Answer: " + checksum);
        }

        private static void CountOrbits(List<Planet> planets)
        {
            for (var i = 0; i < planets.Count; i++)
            {
                if (!planets[i].IsRoot)
                {
                    planets[i].OrbitTotal = CountPlanetOrbits(planets[i].Orbits, 1);
                }
                
            }
        }

        private static int CountPlanetOrbits(Planet orbits, int noOfOrbits)
        {
            while (!orbits.IsRoot)
            {
                noOfOrbits++;
                orbits = orbits.Orbits;
            }

            return noOfOrbits;
        }

        private static void MapOrbits(List<Planet> planets)
        {

            for (var j = 0; j < planets.Count; j++)
            {
                for (var i = 0; i < planets.Count; i++)
                {
                    if (planets[j].OrbitsPlaceholder == planets[i].Name)
                    {
                        planets[j].Orbits = planets[i];

                    }
                    else if (planets[j].Name == "COM")
                    {

                    }
                }

            }
        }
    }

    public class Planet
    {
        public string OrbitsPlaceholder { get; set; }
        public Planet Orbits { get; set; }
        public string Name { get; set; }
        public int OrbitTotal { get; set; }
        public bool IsRoot { get; set; }

    }
}
