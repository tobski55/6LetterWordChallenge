using XLetterWordChallenge;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ReadWordsFromFile_FileExists_ReturnsListOfWords()
        {
            //Arrange
            string filename = "input.txt";
            File.WriteAllText(filename, "apple\nbanana\norange\n");

            //Act
            var words = Program.ReadWordsFromFile(filename);


            //Assert
            Assert.IsNotNull(words);
            Assert.AreEqual(4, words.Count);
            CollectionAssert.Contains(words, "apple");
            CollectionAssert.Contains(words, "banana");
            CollectionAssert.Contains(words, "orange");
            CollectionAssert.Contains(words, "");
        }
        
        [TestMethod]
        public void FindWordCombinations_ThreeWords_MakeOneCombination()
        {
            //Arrange
            List<string> words = ["pen", "penpan","pan"];
            int combinationLength = 6;

            //Act
            var combinations = Program.FindWordCombinations(words, combinationLength);

            //Assert
            Assert.IsNotNull(combinations);
            Assert.AreEqual(1, combinations.Count);
            CollectionAssert.Contains(combinations, "pen + pan = penpan");
            CollectionAssert.DoesNotContain(combinations, "pan + pen = panpen");

        }
    }
}