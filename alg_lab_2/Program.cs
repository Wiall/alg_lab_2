using System;
using System.Collections.Generic;

namespace alg_lab_2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var map = new Dictionary<int, List<int>> {
                {1, new List<int> {10, 15}},                        // Ельзас
                {2, new List<int> {16, 14, 20}},                    // Аквітанія
                {3, new List<int> {5, 7, 13, 14, 16, 22}},          // Овернь
                {4, new List<int> {6, 7, 11, 18}},                  // Нижня Нормандія
                {5, new List<int> {3, 7, 8, 10, 12, 22}},           // Бургундія
                {6, new List<int> {4, 18}},                         // Бретань
                {7, new List<int> {3, 4, 5, 11, 12, 14, 18, 20}},   // Центр — Долина Луари
                {8, new List<int> {5, 10, 12, 15, 19}},             // Шампань-Арденни
                {9, new List<int> {}},                              // Корсика (не має сусідів)
                {10, new List<int> {1, 5, 8, 15, 22}},              // Франш-Конте
                {11, new List<int> {4, 7, 12, 17, 19}},             // Верхня Нормандія
                {12, new List<int> {5, 7, 8, 11, 19}},              // Іль-де-Франс
                {13, new List<int> { 3, 16, 21, 22}},               // Лангедок-Русійон
                {14, new List<int> {2, 3, 7, 16, 20}},              // Лімузен
                {15, new List<int> {1, 8, 10}},                     // Лотарингія
                {16, new List<int> {2, 3, 13, 14}},                 // Південь-Піренеї
                {17, new List<int> {19}},                           // Нор-Па-де-Кале
                {18, new List<int> {4, 6, 7, 20}},                  // Пеї-де-ла-Луар
                {19, new List<int> {8, 11, 12, 17}},                // Пікардія
                {20, new List<int> {2, 7, 14, 18}},                 // Пуату-Шарант
                {21, new List<int> {13, 22}},                       // Прованс — Альпи — Лазурний Берег
                {22, new List<int> {3, 5, 10, 13, 21}},             // Рона-Альпи
            };
            
            int numOfColors = 4;
            Console.WriteLine("Choose the region code (from 1 to 22)");
            int regionNum = int.Parse(Console.ReadLine());
            
            Console.WriteLine("1 - AnnealMapColoring(ANNEAL), 2 - BacktrackingMapColoring(MRV)");
            switch (Console.ReadLine())
            {
                case "1":
                    var solver2 = new AnnealMapColoring(map, numOfColors, regionNum);
                    var solution = solver2.Solve();

                    Console.WriteLine("Final solution:");
                    solver2.PrintSolution();
                    return;
                
                case "2":
                    var solver = new BacktrackingMapColoring(map, numOfColors);
                    bool success = solver.Backtracking(regionNum);

                    if (success)
                    {
                        Console.WriteLine("Solution found:");
                        solver.PrintSolution();
                    }
                    else
                    {
                        Console.WriteLine("No solution found.");
                    }
                    return;
                
                default:
                    Console.WriteLine("Error");
                    return;
            }
        }
    }
}