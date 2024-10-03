using System;
using System.Collections.Generic;
using System.Linq;

namespace alg_lab_2
{
    public class AnnealMapColoring
    {
        private Dictionary<int, List<int>> map;
        private Dictionary<int, int> colors;
        private double temperature = 1000;
        private int maxIterations = 1000000;
        private double coolingRate = 0.001;
        private Random random = new Random();
        private int numOfColors;

        private int steps = 0;
        private int deadEnds = 0;
        private int generatedStates = 0;
        private int storedStates = 0;

        public AnnealMapColoring(Dictionary<int, List<int>> map, int numOfColors, int startRegion)
        {
            this.map = map;
            this.numOfColors = numOfColors;
            this.colors = GenerateRandomSolutionWithStart(startRegion);
        }

        public Dictionary<int, int> Solve()
        {
            while (steps < maxIterations && temperature > 0)
            {
                steps++;

                Dictionary<int, int> newSolution = GenerateNeighborSolution();
                generatedStates++;

                int currentCost = CalculateConflicts(colors);
                int newCost = CalculateConflicts(newSolution);

                if (AcceptanceProbability(currentCost, newCost, temperature) > random.NextDouble())
                {
                    colors = newSolution;
                }
                else
                {
                    deadEnds++;
                }
                
                temperature = 1000 - coolingRate * steps;

                storedStates = Math.Max(storedStates, colors.Count);

                if (CalculateConflicts(colors) == 0) break;
            }

            return colors;
        }
        
        private Dictionary<int, int> GenerateNeighborSolution()
        {
            var newColors = new Dictionary<int, int>(colors);

            var regionKeys = map.Keys.ToList();
            int randomIndex = random.Next(regionKeys.Count);
            int region = regionKeys[randomIndex];

            int newColor;
            do
            {
                newColor = random.Next(1, numOfColors + 1);
            } while (newColors[region] == newColor);

            newColors[region] = newColor;
            return newColors;
        }

        private int CalculateConflicts(Dictionary<int, int> solution)
        {
            int conflicts = 0;
            foreach (var region in map.Keys)
            {
                foreach (var neighbor in map[region])
                {
                    if (solution[region] == solution[neighbor])
                        conflicts++;
                }
            }

            return conflicts / 2;
        }

        private double AcceptanceProbability(int currentCost, int newCost, double temp)
        {
            if (newCost < currentCost) return 1.0;
            return Math.Exp((currentCost - newCost) / temp);
        }

        private Dictionary<int, int> GenerateRandomSolutionWithStart(int startRegion)
        {
            var randomSolution = new Dictionary<int, int>();

            foreach (var region in map.Keys)
            {
                randomSolution[region] = random.Next(1, numOfColors + 1);
            }

            randomSolution[startRegion] = random.Next(1, numOfColors + 1);

            return randomSolution;
        }

        public void PrintSolution()
        {
            foreach (var regionColor in colors)
            {
                Console.WriteLine($"Region {regionColor.Key} has color {regionColor.Value}");
            }
            MapColoringResultSaver.SaveResult(colors, "map_coloring_solution.json");

            Console.WriteLine($"\nSteps (Iterations): {steps}");
            Console.WriteLine($"Dead Ends: {deadEnds}");
            Console.WriteLine($"Generated States: {generatedStates}");
            Console.WriteLine($"Max Stored States in Memory: {storedStates}");
        }
    }
}
