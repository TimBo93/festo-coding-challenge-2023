namespace FestoCodingChallenge2023.Chapter_2;

public class Chapter2Puzzle3
{
    [Fact]
    public void Puzzle3()
    {
        var traps = File.ReadAllLines("Chapter 2/Assets/23_trap_right_side.txt").Select(Trap.Parse)
            .ToList();

        var sumOfValidIds = traps.Where(x => x.CanBeBalanced()).Sum(x => x.TrapId);
    }


    [Theory]
    [InlineData("0: 2 6 210 - X X X", false)]
    [InlineData("0: 2 5 190 - X X X", false)]
    [InlineData("0: 3 54 - X X", false)]
    [InlineData("0: 2 7 70 - X X X", false)]
    [InlineData("0: 8 14 - X X", false)]
    [InlineData("0: 2 3 33 - X X X", false)]
    [InlineData("0: 42 1337 - X X", false)]
    [InlineData("0: 3 105 - X X", true)]
    [InlineData("0: 5 30 310 - X X X", true)]
    [InlineData("0: 3 12 - X X", true)]
    [InlineData("0: 35 6090 - X X", true)]
    [InlineData("0: 2 30 - X X", true)]
    [InlineData("0: 5 195 - X X", true)]
    private void CanTrapBeBalanced(string line, bool result)
    {
        var trap = Trap.Parse(line);
        Assert.Equal(result, trap.CanBeBalanced());
    }

    [Fact]
    private void FractionSetAddsCorrectly()
    {
        var a = new Fraction(1, 2);
        var b = new Fraction(3, 4);

        var sum = a.Add(b);
        Assert.Equal(10, sum.Numerator);
        Assert.Equal(8, sum.Denumerator);
    }

    [Fact]
    private void FractionSubtractCorrectly()
    {
        var a = new Fraction(3, 4);
        var b = new Fraction(1, 8);

        var sum = a.Subtract(b);
        Assert.Equal(20, sum.Numerator);
        Assert.Equal(32, sum.Denumerator);
    }

    [Fact]
    private void FractionsAreComparedCorrectly()
    {
        var a = new Fraction(3, 6);
        var b = new Fraction(5, 6);

        Assert.True(a.CompareTo(b) < 0);
        Assert.True(b.CompareTo(a) > 0);
        Assert.True(a.CompareTo(a) == 0);
    }

    [Fact]
    private void IsValidAsWeight()
    {
        var a = new Fraction(3, 9);
        var b = new Fraction(2, 3);

        Assert.True(a.IsValidAsWeight());
        Assert.False(b.IsValidAsWeight());
    }
}

internal class Trap
{
    public required int TrapId { get; init; }
    public required IReadOnlyList<decimal> Weights { get; init; }
    public required int NumWeightsToSet { get; init; }

    public static Trap Parse(string line)
    {
        var split = line.Split(":");
        var trapId = int.Parse(split[0].Trim());

        var configSplit = split[1].Split("-");

        var numWeightsToSet = configSplit[1].Split(" ").Count(x => x == "X");

        var weights = configSplit[0].Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(decimal.Parse)
            .ToList();

        return new Trap
        {
            NumWeightsToSet = numWeightsToSet,
            TrapId = trapId,
            Weights = weights
        };
    }

    public bool CanBeBalanced()
    {
        var sum = Weights.Select(x => new Fraction(1, x))
            .Aggregate(Fraction.Zero, (accumulate, fraction) => accumulate.Add(fraction));

        return CanBeFilledExactly(Weights.Count, sum, 1);
    }

    internal bool CanBeFilledExactly(int fractionsLeft, Fraction fractionToFill, decimal minDenumerator)
    {
        if (fractionToFill.CompareTo(Fraction.Zero) < 0) return false;

        if (fractionsLeft == 1)
        {
            if (!fractionToFill.IsValidAsWeight()) return false;
            
            var finalWeight = fractionToFill.Denumerator / fractionToFill.Numerator;
            if (finalWeight >= minDenumerator)
            {
                return !Weights.Contains(finalWeight);
            }

            return false;

        }

        for (var weight = minDenumerator;; weight++)
        {
            if (Weights.Contains(weight)) continue;

            var weightFraction = new Fraction(1, weight);

            var upperBound = weightFraction.Times(fractionsLeft);
            if (upperBound.CompareTo(fractionToFill) < 0) return false;

            if (CanBeFilledExactly(fractionsLeft - 1, fractionToFill.Subtract(weightFraction), weight + 1)) return true;
        }
    }
}

internal class Fraction : IComparable<Fraction>
{
    public Fraction(decimal numerator, decimal denumerator)
    {
        Numerator = numerator;
        Denumerator = denumerator;
    }

    public decimal Numerator { get; }

    public decimal Denumerator { get; }

    public static Fraction Zero => new(0, 1);

    public int CompareTo(Fraction? other)
    {
        var thisNormalizedNumerator = Numerator * other.Denumerator;
        var otherNormalizedNumerator = Denumerator * other.Numerator;

        if (thisNormalizedNumerator < otherNormalizedNumerator) return -1;

        if (thisNormalizedNumerator > otherNormalizedNumerator) return 1;

        return 0;
    }

    public bool IsValidAsWeight()
    {
        return Denumerator % Numerator == 0;
    }

    public Fraction Add(Fraction fraction)
    {
        var commonBase = Denumerator * fraction.Denumerator;
        return new Fraction(Numerator * fraction.Denumerator + fraction.Numerator * Denumerator, commonBase);
    }

    public Fraction Subtract(Fraction fraction)
    {
        return Add(fraction.Negate());
    }

    public Fraction Negate()
    {
        return new Fraction(-Numerator, Denumerator);
    }

    public Fraction Times(int fractionsLeft)
    {
        return new Fraction(Numerator * fractionsLeft, Denumerator);
    }
}