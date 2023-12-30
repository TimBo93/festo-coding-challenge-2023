using System.Collections.ObjectModel;
using System.Reflection.Metadata.Ecma335;
using FestoCodingChallenge2023.Shared;

namespace FestoCodingChallenge2023.Chapter_3;

public class Chapter3Puzzle1
{
    private readonly List<Hammer> _hammers;

    public Chapter3Puzzle1()
    {
        _hammers = File.ReadAllLines("Chapter 3/Assets/hammer_collection.txt").Select(Hammer.Parse)
            .ToList();
    }

    [Fact]
    public void FindTheOneForgableKey()
    {
        var forges = File.ReadAllLines("Chapter 3/Assets/31_keymaker_forge_2.txt").ToList();
        var forgableForge = forges.Single(IsValid);
    }

    [Fact]
    public void TrivialForgeIsValid()
    {
        Assert.True(IsValid("A"));
    }

    [Fact]
    public void SimpleForgeIsValid()
    {
        Assert.True(IsValid("BC"));
    }

    [Fact]
    public void BitMoreComplexForgeIsValid()
    {
        Assert.True(IsValid("BDC"));
    }

    [Fact]
    public void ComplexForgeIsValid()
    {
        Assert.True(IsValid("BFBCC"));
    }

    [Fact]
    public void TrivialForgeIsInvalid()
    {
        Assert.False(IsValid("B"));
    }

    [Fact]
    public void ComplexForgeIsInvalid()
    {
        Assert.False(IsValid("AFAF"));
    }


    private bool IsValid(string forge)
    {
        Console.WriteLine(forge);
        SolverContext context = new(_hammers);
        return context.IsValid(forge);
    }
}

internal class SolverContext
{
    private readonly ReadOnlyDictionary<string, Hammer> _hammerByTarget;
    private static readonly HashSet<string> _invalidForges = new();

    public SolverContext(IReadOnlyList<Hammer> hammers)
    {
        _hammerByTarget = hammers.ToDictionary(x => x.To).AsReadOnly();
    }

    //private bool CanBeReduced(string forge)
    //{
    //    // CDDDDFECBFFFCBCFFABDCBFFBFE
    //    if (forge == "A") return true;

    //    if (forge.Length == 1)
    //    {
    //        return false;
    //}

    public bool IsValid(string forge)
    {
        if (forge == "A")
        {
            return true;
        }

        foreach (var hammer in _hammerByTarget.Values)
        {
            var neighbors = ApplyHammerToForge(hammer, forge);
            foreach (var neighbor in neighbors)
            {
                if(_invalidForges.Contains(neighbor))
                {
                    continue;
                }

                if (IsValid(neighbor))
                {
                    return true;
                }
            }
        }

        _invalidForges.Add(forge);
        return false;
    }

    private Hammer? FindHammer(string target)
    {
        if (_hammerByTarget.TryGetValue(target, out var hammer)) return hammer;

        return null;
    }

    private IEnumerable<string> ApplyHammerToForge(Hammer hammer, string forge)
    {
        var currentIndex = 0;
        while (true)
        {
            var index = forge.IndexOf(hammer.To, currentIndex);
            if (index == -1) break;
            currentIndex = index + 1;

            var insert = forge.Insert(index, hammer.From);
            var applyHammerToForge = insert.Remove(index + hammer.From.Length, hammer.To.Length);
            yield return applyHammerToForge;
        }
    }
}