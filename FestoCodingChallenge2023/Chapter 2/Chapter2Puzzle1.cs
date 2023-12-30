using FestoCodingChallenge2023.Shared;

namespace FestoCodingChallenge2023.Chapter_2;

public class Chapter2Puzzle1
{
    [Fact]
    public void Puzzle3()
    {
        var hammers = File.ReadAllLines("Chapter 2/Assets/hammer_collection.txt").Select(Hammer.Parse)
            .ToList();
        var forges = File.ReadAllLines("Chapter 2/Assets/21_keymaker_forge.txt").ToList();

        var maxLength = forges.Max(x => x.Length);

        var dictionaryList = new List<HashSet<string>>();
        for (int i = 0; i < maxLength; i++)
        {
            dictionaryList.Add(new HashSet<string>());
        }

        dictionaryList[0].Add("A");

        for (var i = 0; i < maxLength - 1; i++)
        {
            foreach (var item in dictionaryList[i])
            {
                foreach (var hammer in hammers)
                {
                    var currentIndex = 0;
                    while (true)
                    {
                        var index = item.IndexOf(hammer.From, currentIndex);
                        if (index == -1)
                        {
                            break;
                        }
                        currentIndex = index + 1;

                        var newString = item.Insert(index, hammer.To).Remove(index + hammer.To.Length, 1);
                        dictionaryList[i + 1].Add(newString);
                    }
                }
            }
        }

        var containedKey = forges.Single(x => dictionaryList[x.Length - 1].Contains(x));

        var x = 0;
    }
}