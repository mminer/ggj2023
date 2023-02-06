using RogueSharp.Random;
using System;

/// <summary>
/// Wraps a random number generator to use with RogueSharp. And any other time randomness affects gameplay.
/// </summary>
public class RandomNumberGenerator : IRandom
{
    public int randomSeed { get; private set; }

    Random random;
    long timesUsed;

    public RandomNumberGenerator(int randomSeed)
    {
        this.random = new Random(randomSeed);
        this.randomSeed = randomSeed;
    }

    public int Next(int minValue, int maxValue)
    {
        timesUsed++;
        return random.Next(minValue, maxValue);
    }

    public int Next(int maxValue)
    {
        timesUsed++;
        return random.Next(maxValue);
    }

    public bool NextBool()
    {
        timesUsed++;
        return random.Next(2) == 1;
    }

    public double NextDouble()
    {
        timesUsed++;
        return random.NextDouble();
    }

    public void Restore(RandomState state)
    {
        randomSeed = state.Seed[0];
        random = new Random(randomSeed);
        timesUsed = state.NumberGenerated;
    }

    public RandomState Save()
    {
        return new RandomState
        {
            NumberGenerated = timesUsed,
            Seed = new[] { randomSeed },
        };
    }

    public override string ToString()
    {
        return $"seed: {randomSeed}; times used: {timesUsed}";
    }
}
