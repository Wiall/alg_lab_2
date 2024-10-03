using System;
using System.Collections.Generic;
using System.Linq;

namespace alg_lab_2
{
    public class BacktrackingMapColoring
    {
        private Dictionary<int, List<int>> map;
        private Dictionary<int, int> colors;
        private int numOfColors;
        private List<int> uncoloredRegions;

        private int steps = 0;
        private int deadEnds = 0;
        private int generatedStates = 0;
        private int storedStates = 0;

        public BacktrackingMapColoring(Dictionary<int, List<int>> map, int numOfColors)
        {
            this.map = map;
            this.numOfColors = numOfColors;
            this.colors = new Dictionary<int, int>();
            this.uncoloredRegions = map.Keys.ToList();
        }

        public bool Backtracking(int startRegion)
        {
            if (uncoloredRegions.Contains(startRegion))
            {
                uncoloredRegions.Remove(startRegion);
                if (Solve(startRegion)) return true;
            }
            return Solve();
        }

        private bool Solve(int? region = null)
        {
            steps++;
            if (region == null) region = SelectRegionWithMRV();
            if (region == -1) return true;

            for (int color = 1; color <= numOfColors; color++)
            {
                if (IsValid(region.Value, color))
                {
                    colors[region.Value] = color;
                    uncoloredRegions.Remove(region.Value);
                    generatedStates++;

                    storedStates = Math.Max(storedStates, colors.Count);

                    if (Solve()) return true;

                    colors.Remove(region.Value);
                    uncoloredRegions.Add(region.Value);
                    deadEnds++;
                }
            }

            return false;
        }

        private int SelectRegionWithMRV()
        {
            int selectedRegion = -1;
            int minAvailableColors = int.MaxValue;

            foreach (int region in uncoloredRegions)
            {
                int availableColors = GetAvailableColors(region).Count;
                if (availableColors < minAvailableColors)
                {
                    minAvailableColors = availableColors;
                    selectedRegion = region;
                }
            }

            return selectedRegion;
        }

        private bool IsValid(int region, int color)
        {
            foreach (int neighbor in map[region])
            {
                if (colors.ContainsKey(neighbor) && colors[neighbor] == color)
                {
                    return false;
                }
            }

            return true;
        }

        private List<int> GetAvailableColors(int region)
        {
            HashSet<int> neighborColors = new HashSet<int>();

            foreach (int neighbor in map[region])
            {
                if (colors.ContainsKey(neighbor))
                {
                    neighborColors.Add(colors[neighbor]);
                }
            }

            List<int> availableColors = new List<int>();
            for (int color = 1; color <= numOfColors; color++)
            {
                if (!neighborColors.Contains(color))
                {
                    availableColors.Add(color);
                }
            }

            return availableColors;
        }

        public void PrintSolution()
        {
            if (colors.Count == 0)
            {
                Console.WriteLine("No solution found.");
            }
            else
            {
                foreach (var regionColor in colors)
                {
                    Console.WriteLine($"Region {regionColor.Key} has color {regionColor.Value}");
                }
            }
            MapColoringResultSaver.SaveResult(colors, "map_coloring_solution.json");

            Console.WriteLine($"\nSteps (Iterations): {steps}");
            Console.WriteLine($"Dead Ends: {deadEnds}");
            Console.WriteLine($"Generated States: {generatedStates}");
            Console.WriteLine($"Max Stored States in Memory: {storedStates}");
        }
    }
}
