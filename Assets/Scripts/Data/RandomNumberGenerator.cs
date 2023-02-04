using RogueSharp.Random;
using UnityEngine;

/// <summary>
/// Wraps Unity's random number generator to use with RogueSharp.
/// </summary>
public class RandomNumberGenerator : IRandom
{
    public int randomSeed { get; private set; }

    long numberGenerated;

    public RandomNumberGenerator(int randomSeed)
    {
        this.randomSeed = randomSeed;
        Random.InitState(randomSeed);
    }

    public int Next(int minValue, int maxValue)
    {
        numberGenerated++;
        return Random.Range(minValue, maxValue);
    }

    public int Next(int maxValue)
    {
        return Next(0, maxValue);
    }

    public bool NextBool()
    {
        return Next(0, 2) == 1;
    }

    public void Restore(RandomState state)
    {
        numberGenerated = state.NumberGenerated;
        randomSeed = state.Seed[0];
        Random.InitState(randomSeed);
    }

    public RandomState Save()
    {
        return new RandomState
        {
            NumberGenerated = numberGenerated,
            Seed = new[] { randomSeed },
        };
    }
}
