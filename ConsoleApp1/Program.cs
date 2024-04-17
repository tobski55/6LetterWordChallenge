using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace XLetterWordChallenge
{
    public class Program
    {
        static void Main(string[] args)
        {
            string sourceInputData = "..\\input.txt";
            //for debugging purposes
            //string sourceInputData = "..\\..\\..\\..\\input.txt";

            int combinationLength = 6;

            List<string> words = ReadWordsFromFile(sourceInputData);
            List<string> wordCombinations = FindWordCombinations(words.OrderByDescending(word => word.Length).ToList(), combinationLength, 0);

            Console.WriteLine($"{combinationLength}-Letter Words:");
            DisplayWordsInRows(words.Where(word => word.Length == combinationLength).ToList(), 14, true);

            Console.WriteLine("\nCombinations:");
            DisplayWordsInRows(wordCombinations, 5, true);

            Console.ReadLine();
        }

        public static List<string> ReadWordsFromFile(string filename)
        {
            List<string> words = new List<string>();

            using (StreamReader sr = new StreamReader(filename))
            {
                var lines = sr.ReadToEnd().Split().ToList();
                foreach (var line in lines)
                    if (line != null && line != "")
                    {
                        words.Add(line.Trim());
                    }
            }

            return words;
        }

        public static List<string> FindWordCombinations(List<string> words, int combinationLength = 6, int amount = 10)
        {
            List<string> combinations = new List<string>();
            List<string> currentCombination = new List<string>();
            int foundCount = 0;

            // Memoization dictionary to store already calculated combinations
            Dictionary<string, bool> memo = new Dictionary<string, bool>();

            // Object for synchronization
            object lockObject = new object();

            if (amount == 0)
            {
                amount = int.MaxValue;
            }

            // Use Parallel.ForEach to parallelize the loop
            Parallel.ForEach(words, word =>
            {
                List<string> singleWordList = new List<string> { word };
                FindCombinationsRecursive(words, combinationLength, amount, combinations, singleWordList, ref foundCount, memo, lockObject);
            });

            return combinations;
        }


        private static void FindCombinationsRecursive(List<string> words, int combinationLength, int amount, List<string> combinations, List<string> currentCombination, ref int foundCount, Dictionary<string, bool> memo, object lockObject)
        {
            if (foundCount >= amount)
                return;

            string currentCombinationKey = string.Join("_", currentCombination);

            lock (lockObject) // Lock to ensure exclusive access to combinations list
            {
                if (memo.ContainsKey(currentCombinationKey))
                    return;

                if (currentCombination.Sum(word => word.Length) == combinationLength)
                {
                    string combination = $"{string.Join(" + ", currentCombination)} = {string.Concat(currentCombination)}";
                    if (words.Contains(string.Concat(currentCombination)) && !combinations.Contains(combination) && currentCombination.Count() > 1)
                    {
                        combinations.Add(combination);
                        Console.WriteLine($"Found combination: {combination}");
                        foundCount++;
                    }
                    memo[currentCombinationKey] = true; // Memoize the current combination
                    return;
                }

                if (currentCombination.Sum(word => word.Length) > combinationLength)
                {
                    memo[currentCombinationKey] = false; // Memoize the current combination as invalid
                    return;
                }
            }

            foreach (string word in words)
            {
                if (!currentCombination.Contains(word))
                {
                    currentCombination.Add(word);
                    FindCombinationsRecursive(words, combinationLength, amount, combinations, currentCombination, ref foundCount, memo, lockObject);
                    currentCombination.RemoveAt(currentCombination.Count - 1); // Backtrack
                }
            }
        }


        public static void DisplayWordsInRows(List<string> words, int wordsPerRow, bool unique = false)
        {
            if (unique)
            {
                words = words.Distinct().ToList();
            }

            for (int i = 0; i < words.Count; i += wordsPerRow)
            {
                Console.WriteLine(string.Join("\t", words.Skip(i).Take(wordsPerRow)));
            }
        }
    }
}
