namespace FestoCodingChallenge2023.Shared;

public class Hammer
{
    public required int Number { get; init; }
    public required string From { get; init; }
    public required string To { get; init; }

    public static Hammer Parse(string line)
    {
        var split = line.Split(" ");
        return new Hammer
        {
            Number = int.Parse(split[0].Replace(".", "")),
            From = split[1],
            To = split[3]
        };
    }
}