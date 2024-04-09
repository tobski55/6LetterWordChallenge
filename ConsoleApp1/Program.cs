using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace WordCombinationFinder
{
    public class Program
    {
        static void Main(string[] args)
        {
            string sourceInputData = "input.txt";
            int combinationLength = 6;

            List<string> words = ReadWordsFromFile(sourceInputData);
            List<string> wordCombinations = FindWordCombinations(words, combinationLength, 200);


            Console.WriteLine("6-Letter Words:");
            // DisplayWordsInRows(words, 20);
            DisplayWordsInRows(words.Where(word => word.Length == 6).ToList(), 20);

            Console.WriteLine("\nCombinations:");
            // List<string> vallidCombinations = FilterValidCombinations(wordCombinations, words);
            List<string> uniquevallidCombinations = FilterValidCombinations(wordCombinations, true);

            DisplayWordsInRows(uniquevallidCombinations, 10);

            Console.ReadLine();

        }

        public static List<string> ReadWordsFromFile(string filename)
        {
            List<string> words = new List<string>();

            using (StreamReader sr = new StreamReader(filename))
            {
                var lines = sr.ReadToEnd().Split().ToList();
                foreach (var line in lines)
                    if (line != null)
                    {
                        words.Add(line.Trim());
                    }
            }

            return words;
        }

        public static List<string> FindWordCombinations(List<string> words,int combinationLength = 6, int amount = 10)
        {
            List<string> combinations = new List<string>();
            int count = 0;

            for (int i = 0; i < words.Count && count < amount; i++)
            {
                for (int j = i + 1; j < words.Count && count < amount; j++)
                {
                    string combined = words[i] + words[j];
                    if (IsValidCombination(words[i], words[j], words, combinationLength))
                    {
                        combinations.Add($"{words[i]} + {words[j]} = {combined}");
                        count++;
                    }

                    combined = words[j] + words[i];
                    if (IsValidCombination(words[j], words[i], words, combinationLength) && count < amount)
                    {
                        combinations.Add($"{words[j]} + {words[i]} = {combined}");
                        count++;
                    }
                }
            }

            return combinations;
        }

        public static bool IsValidCombination(string word1, string word2, List<string> words, int combinationLength)
        {
            if (word1 == "" || word2 == "") return false;

            string combination1 = word1 + word2;
            string combination2 = word2 + word1;

            return words.Contains(combination1) && combination1.Length == combinationLength || words.Contains(combination2) && combination2.Length == combinationLength;
        }

        public static void DisplayWordsInRows(List<string> words, int wordsPerRow)
        {
            for (int i = 0; i < words.Count; i += wordsPerRow)
            {
                Console.WriteLine(string.Join("\t", words.Skip(i).Take(wordsPerRow)));
            }
        }

        public static List<string> FilterValidCombinations(List<string> combinations, bool unique = false)
        {

            return !unique ? combinations : combinations.Distinct().ToList();
        }
    }
}
