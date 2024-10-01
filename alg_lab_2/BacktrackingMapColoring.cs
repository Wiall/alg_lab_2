using System;
using System.Collections.Generic;
using System.Linq;

namespace alg_lab_2
{
    public class BacktrackingMapColoring
    {
        private Dictionary<int, List<int>> map; // Карта з регіонами та сусідніми зв'язками
        private Dictionary<int, int> colors; // Кольори для кожного регіону
        private int numOfColors;
        private List<int> uncoloredRegions;

        // Додані лічильники
        private int steps = 0; // Кількість ітерацій (етапів)
        private int deadEnds = 0; // Кількість глухих кутів
        private int generatedStates = 0; // Кількість згенерованих станів
        private int storedStates = 0; // Кількість станів у пам'яті

        public BacktrackingMapColoring(Dictionary<int, List<int>> map, int numOfColors)
        {
            this.map = map;
            this.numOfColors = numOfColors;
            this.colors = new Dictionary<int, int>();
            this.uncoloredRegions = map.Keys.ToList(); // Всі регіони початково не пофарбовані
        }

        public bool Backtracking(int startRegion)
        {
            // Починаємо з обраного користувачем міста
            if (uncoloredRegions.Contains(startRegion))
            {
                uncoloredRegions.Remove(startRegion);
                if (Solve(startRegion)) return true;
            }

            return Solve();
        }

        private bool Solve(int? region = null)
        {
            steps++; // Кожен рекурсивний виклик – це новий крок
            if (region == null) region = SelectRegionWithMRV(); // Якщо регіон не обраний, вибираємо за MRV
            if (region == -1) return true; // Всі регіони пофарбовані

            for (int color = 1; color <= numOfColors; color++)
            {
                if (IsValid(region.Value, color))
                {
                    colors[region.Value] = color; // Присвоєння кольору регіону
                    uncoloredRegions.Remove(region.Value); // Видалення регіону зі списку нефарбованих
                    generatedStates++; // Новий стан згенеровано

                    // Оновлення кількості станів, що зберігаються (у даний момент часу)
                    storedStates = Math.Max(storedStates, colors.Count);

                    if (Solve()) return true;

                    // Відміна вибору (backtracking)
                    colors.Remove(region.Value);
                    uncoloredRegions.Add(region.Value); // Повернення регіону до нефарбованих
                    deadEnds++; // Це глухий кут
                }
            }

            return false; // Не знайдено рішення
        }

        // Вибір регіону з найменшою кількістю доступних кольорів (MRV)
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

            return selectedRegion; // Повертаємо -1, якщо немає регіонів для вибору
        }

        // Перевірка, чи можна присвоїти колір регіону, враховуючи кольори сусідів
        private bool IsValid(int region, int color)
        {
            foreach (int neighbor in map[region])
            {
                if (colors.ContainsKey(neighbor) && colors[neighbor] == color)
                {
                    return false; // Конфлікт, сусід має той самий колір
                }
            }

            return true; // Конфліктів немає
        }

        // Отримання списку доступних кольорів для даного регіону
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

            // Повертаємо кольори, які ще не використовуються сусідами
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

        // Виведення кількості кроків, глухих кутів, станів та результату
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

            // Виведення статистики
            Console.WriteLine($"\nSteps (Iterations): {steps}");
            Console.WriteLine($"Dead Ends: {deadEnds}");
            Console.WriteLine($"Generated States: {generatedStates}");
            Console.WriteLine($"Max Stored States in Memory: {storedStates}");
        }
    }
}
