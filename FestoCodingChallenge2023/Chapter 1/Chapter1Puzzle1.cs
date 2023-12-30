using System.Collections.Immutable;
using FestoCodingChallenge2023.Shared;

namespace FestoCodingChallenge2023.Chapter_1;

public class Chapter1Puzzle1
{
    [Fact]
    public void Puzzle1()
    {
        var hammers = File.ReadAllLines("Chapter 1/Assets/hammer_collection.txt").Select(Hammer.Parse).ToDictionary(x => x.Number);

        var keys = File.ReadAllLines("Chapter 1/Assets/11_keymaker_recipe.txt").Select(x =>
        {
            var parts = x.Replace(" ", "").Replace("(", "").Replace(")", "").Split("-");

            var instructionList = parts.Select(part => part.Split(",")).Select(numbers =>
                new Instruction { Index = int.Parse(numbers[0]), Position = int.Parse(numbers[1]) }).ToList();
            return instructionList;
        }).ToImmutableList();

        var allValidKeys = keys.Select(key =>
        {
            var currentKey = "A";
            foreach (var instruction in key)
            {
                var hammerFound = hammers.TryGetValue(instruction.Index, out var hammer);
                if (!hammerFound) return null;

                if (currentKey.Length < instruction.Position) return null;

                if (currentKey[instruction.Position - 1].ToString() != hammer!.From) return null;

                currentKey = currentKey.Remove(instruction.Position - 1, 1);
                currentKey = currentKey.Insert(instruction.Position - 1, hammer.To);
            }

            return currentKey;
        }).Where(x => x != null).ToList();

        var x = allValidKeys;
    }
}

public class Instruction
{
    public required int Index { get; init; }
    public required int Position { get; init; }
}