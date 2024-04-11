namespace XLetterWordChallenge
{
    public class Program
    {
        static void Main(string[] args)
        {
            string sourceInputData = "..\\..\\..\\..\\input.txt";

            int combinationLength = 6;

            List<string> words = ReadWordsFromFile(sourceInputData);
            List<string> wordCombinations = FindWordCombinations(words, combinationLength, 200);


            Console.WriteLine($"{combinationLength}-Letter Words:");
            DisplayWordsInRows(words.Where(word => word.Length == 6).ToList(), 14, true);

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

            string combination = word1 + word2;

            return words.Contains(combination) && combination.Length == combinationLength;
        }

        public static void DisplayWordsInRows(List<string> words, int wordsPerRow, bool unique = false)
        {
            if(unique)
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
