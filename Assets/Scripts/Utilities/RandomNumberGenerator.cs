using RogueSharp.Random;
using UnityEngine;

/// <summary>
/// Wraps Unity's random number generator to use with RogueSharp.
/// </summary>
public class RandomNumberGenerator : IRandom
{
    int seed;
    long timesUsed;

    public RandomNumberGenerator(int seed)
    {
        this.seed = seed;
        Random.InitState(seed);
    }

    public int Next(int maxValue)
    {
        return Random.Range(0, maxValue);
    }

    public int Next(int minValue, int maxValue)
    {
        return Random.Range(minValue, maxValue);
    }

    public bool NextBool()
    {
        return Random.Range(0, 2) == 1;
    }

    public void Restore(RandomState state)
    {
        seed = state.Seed[0];
        timesUsed = state.NumberGenerated;
        Random.InitState(seed);
    }

    public RandomState Save()
    {
        return new RandomState
        {
            NumberGenerated = timesUsed,
            Seed = new[] { seed },
        };
    }
}
