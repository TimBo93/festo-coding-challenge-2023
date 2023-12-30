using System.Collections.Immutable;
using Xunit.Abstractions;

namespace FestoCodingChallenge2023.Tutorial;

public class TutorialPuzzle3
{
    private readonly ITestOutputHelper _testOutputHelper;

    public TutorialPuzzle3(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Puzzle3()
    {
        var lines = File.ReadAllLines("Tutorial/Assets/03_trap_logs.txt");

        var rules = new RuleSet
        {
            DisabledKeywords = "inactive, disabled, quiet, standby, idle".Split(", "),
            EnabledKeywords = "live, armed, ready, primed, active".Split(", "),
            FlippingKeywords = "flipped, toggled, reversed, inverted, switched".Split(", ")
        };

        var sumOfNumbers = lines.Select(x => ParseLine(x, rules)).Where(x =>
        {
            return x.Tokens.Aggregate(State.Unknown, (current, next) =>
            {
                return (current, next) switch
                {
                    (State.Unknown, Token.Disable) => State.Disabled,
                    (State.Unknown, Token.Enable) => State.Enabled,
                    (State.Unknown, Token.Flip) => State.Unknown,
                    (State.Disabled, Token.Flip) => State.Enabled,
                    (State.Enabled, Token.Flip) => State.Disabled,
                    (_, Token.Disable) => State.Disabled,
                    (_, Token.Enable) => State.Enabled
                };
            }) == State.Disabled;
        }).Sum(x => x.LineNumber);

        _testOutputHelper.WriteLine($"Solution of Tutorial Puzzle 3 is: {sumOfNumbers}");

        Assert.True(true);
    }

    private Line ParseLine(string line, RuleSet ruleSet)
    {
        var tokens = line.Split(" ").Where(x => !string.IsNullOrEmpty(x)).ToImmutableList();

        return new Line
        {
            LineNumber = int.Parse(tokens[0].Replace(":", "")),
            Tokens = tokens.Skip(1).Select(x =>
            {
                if (ruleSet.EnabledKeywords.Contains(x)) return Token.Enable;

                if (ruleSet.DisabledKeywords.Contains(x)) return Token.Disable;

                if (ruleSet.FlippingKeywords.Contains(x)) return Token.Flip;

                throw new NotSupportedException(x);
            }).ToArray()
        };
    }

    private enum State
    {
        Enabled,
        Disabled,
        Unknown
    }

    private enum Token
    {
        Enable,
        Disable,
        Flip
    }

    private record Line
    {
        public required int LineNumber { get; init; }
        public required Token[] Tokens { get; init; }
    }

    private record RuleSet
    {
        public string[] DisabledKeywords { get; init; }
        public string[] EnabledKeywords { get; init; }
        public string[] FlippingKeywords { get; init; }
    }
}