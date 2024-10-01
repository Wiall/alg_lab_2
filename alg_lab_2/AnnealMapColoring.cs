using System;
using System.Collections.Generic;
using System.Linq;

namespace alg_lab_2
{
    public class AnnealMapColoring
    {
        private Dictionary<int, List<int>> map; // Карта з регіонами та сусідніми зв'язками
        private Dictionary<int, int> colors; // Кольори для кожного регіону
        private double temperature = 1000; // Початкова температура
        private double coolingRate = 0.01; // Коефіцієнт охолодження
        private Random random = new Random(); // Генератор випадкових чисел
        private int numOfColors;

        // Додані змінні для лічильників
        private int steps = 0; // Кількість етапів (кроків)
        private int deadEnds = 0; // Кількість випадків глухих кутів
        private int generatedStates = 0; // Кількість згенерованих станів
        private int storedStates = 0; // Кількість станів, що зберігаються в пам'яті

        public AnnealMapColoring(Dictionary<int, List<int>> map, int numOfColors, int startRegion)
        {
            this.map = map;
            this.numOfColors = numOfColors;
            this.colors = GenerateRandomSolutionWithStart(startRegion); // Генерація початкового рішення з обраним стартом
        }

        public Dictionary<int, int> Solve()
        {
            while (temperature > 1)
            {
                steps++; // Кожен цикл - це новий крок

                Dictionary<int, int> newSolution = GenerateNeighborSolution();
                generatedStates++; // Генерація нового сусіднього стану

                int currentCost = CalculateConflicts(colors);
                int newCost = CalculateConflicts(newSolution);

                if (AcceptanceProbability(currentCost, newCost, temperature) > random.NextDouble())
                {
                    colors = newSolution; // Приймаємо нове рішення
                }
                else
                {
                    deadEnds++; // Якщо рішення не прийнято, це може бути глухий кут
                }

                temperature *= 1 - coolingRate; // Охолоджуємо систему

                // Оновлюємо максимальну кількість станів, що зберігаються
                storedStates = Math.Max(storedStates, colors.Count);

                // Якщо конфліктів немає, завершуємо пошук
                if (CalculateConflicts(colors) == 0) break;
            }

            return colors;
        }

        // Генерація сусіднього рішення (випадкова зміна кольору одного з регіонів)
        private Dictionary<int, int> GenerateNeighborSolution()
        {
            var newColors = new Dictionary<int, int>(colors);

            // Отримання випадкового регіону з існуючих ключів
            var regionKeys = map.Keys.ToList();
            int randomIndex = random.Next(regionKeys.Count); // Отримання випадкового індексу
            int region = regionKeys[randomIndex];            // Вибір випадкового регіону за індексом

            // Випадковий новий колір для регіону, який відрізняється від поточного
            int newColor;
            do
            {
                newColor = random.Next(1, numOfColors + 1);  // Генерація випадкового кольору
            } while (newColors[region] == newColor);         // Уникнення вибору того ж кольору

            newColors[region] = newColor; // Присвоєння нового кольору
            return newColors;
        }

        // Обчислення кількості конфліктів між сусідніми регіонами
        private int CalculateConflicts(Dictionary<int, int> solution)
        {
            int conflicts = 0;
            foreach (var region in map.Keys)
            {
                foreach (var neighbor in map[region])
                {
                    if (solution[region] == solution[neighbor])
                        conflicts++; // Конфлікт, якщо сусіди мають однаковий колір
                }
            }

            return conflicts / 2; // Кожен конфлікт враховується двічі, тому ділимо на 2
        }

        // Ймовірність прийняття нового рішення
        private double AcceptanceProbability(int currentCost, int newCost, double temp)
        {
            if (newCost < currentCost) return 1.0; // Приймаємо краще рішення завжди
            return Math.Exp((currentCost - newCost) / temp); // Приймаємо гірше рішення з певною ймовірністю
        }

        // Генерація початкового рішення з обраним стартовим регіоном
        private Dictionary<int, int> GenerateRandomSolutionWithStart(int startRegion)
        {
            var randomSolution = new Dictionary<int, int>();

            // Генеруємо випадкові кольори для всіх регіонів
            foreach (var region in map.Keys)
            {
                randomSolution[region] = random.Next(1, numOfColors + 1); // Випадковий колір від 1 до numOfColors
            }

            // Додаємо стартовий регіон з випадковим кольором
            randomSolution[startRegion] = random.Next(1, numOfColors + 1); // Випадковий колір для старту

            return randomSolution;
        }

        // Виведення рішення на екран
        public void PrintSolution()
        {
            foreach (var regionColor in colors)
            {
                Console.WriteLine($"Region {regionColor.Key} has color {regionColor.Value}");
            }

            // Виведення статистики
            Console.WriteLine($"\nSteps (Iterations): {steps}");
            Console.WriteLine($"Dead Ends: {deadEnds}");
            Console.WriteLine($"Generated States: {generatedStates}");
            Console.WriteLine($"Max Stored States in Memory: {storedStates}");
        }
    }
}
