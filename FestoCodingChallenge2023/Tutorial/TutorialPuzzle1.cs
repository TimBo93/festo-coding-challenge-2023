using Xunit.Abstractions;

namespace FestoCodingChallenge2023.Tutorial
{
    public static class StringExtensions
    {
        public static bool IsAlphabetic(this string str)
        {
            for (var i = 1; i < str.Length; i++)
            {
                var char1 = str[i - 1];
                var char2 = str[i];

                if (char1 > char2)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class TutorialPuzzle1
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public TutorialPuzzle1(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Puzzle1()
        {
            var file = File.ReadAllLines("Tutorial/Assets/01_keymaker_ordered.txt");
            var orderedKey = file.Single(x => x.IsAlphabetic());
            _testOutputHelper.WriteLine($"Solution of Tutorial Puzzle 1 is: {orderedKey}");
            Assert.True(true);
        }
    }
}
