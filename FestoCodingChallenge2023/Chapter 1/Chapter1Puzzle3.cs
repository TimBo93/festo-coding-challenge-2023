using System.Numerics;

namespace FestoCodingChallenge2023.Chapter_1;

internal static class StringToWeightExtensions
{
    public static BigInteger[] ToWeightArray(this string text)
    {
        return text.Split(" ").Where(x => x.Length > 0).Select(BigInteger.Parse).ToArray();
    }
}

public class Chapter1Puzzle3
{
    [Fact]
    public void Puzzle3()
    {
        var result = File.ReadAllLines("Chapter 1/Assets/13_trap_balance.txt").Select(x =>
        {
            var split = x.Split(":");
            var lineNumber = int.Parse(split[0].Replace(" ", ""));

            var weightSides = split[1].Split("-");
            var weightsLeft = weightSides[0].ToWeightArray();
            var weightsRight = weightSides[1].ToWeightArray();

            return new ParsedTrapLine
            {
                LineNumber = lineNumber,
                WeightsLeft = weightsLeft,
                WeightsRight = weightsRight
            };
        }).Where(x => x.HasEqualWeight()).Sum(x => x.LineNumber);

        Assert.True(new ParsedTrapLine
        {
            WeightsLeft = new BigInteger[] { 4, 20 },
            WeightsRight = new BigInteger[] { 5, 10 },
            LineNumber = 1
        }.HasEqualWeight());

        Assert.False(new ParsedTrapLine
        {
            WeightsLeft = new[] { 2, BigInteger.Parse("99999999999999999999999999999999999") },
            WeightsRight = new BigInteger[] { 3, 6 },
            LineNumber = 1
        }.HasEqualWeight());
    }
}

public class ParsedTrapLine
{
    public required BigInteger[] WeightsLeft { get; init; }
    public required BigInteger[] WeightsRight { get; init; }
    public required int LineNumber { get; init; }

    public bool HasEqualWeight()
    {
        if (WeightsLeft.Length != WeightsRight.Length) return false;

        var concat = WeightsLeft.Concat(WeightsRight).ToArray();
        if (concat.Distinct().Count() != WeightsLeft.Length * 2) return false;

        var left = WeightsLeft.Select(x => NormalizeWeight(x, concat)).Aggregate(new BigInteger(0), (a, b) => a + b);
        var right = WeightsRight.Select(x => NormalizeWeight(x, concat)).Aggregate(new BigInteger(0), (a, b) => a + b);

        Assert.True(left > 0);
        Assert.True(right > 0);

        return left == right;
    }

    private static BigInteger MultiplyAll(BigInteger[] weights)
    {
        return weights.Aggregate(new BigInteger(1),
            (agg, next) => agg * next);
    }

    private static BigInteger NormalizeWeight(BigInteger weight, BigInteger[] concat)
    {
        return concat.Where(c => c != weight)
            .Aggregate(new BigInteger(1), (integer, bigInteger) => integer * bigInteger);
    }
}