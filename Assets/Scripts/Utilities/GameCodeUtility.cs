using System.Collections.Generic;
using System.Globalization;

public static class GameCodeUtility
{
    static readonly Dictionary<char, char> ambiguousCharacterMap = new()
    {
        { '0', 'W' },
        { '5', 'X' },
        { 'B', 'Y' },
    };

    public static int GameCodeToRandomSeed(string gameCode)
    {
        // Swap unambiguous characters with ambiguous hex ones.
        foreach (var (ambiguousChar, unambiguousChar) in ambiguousCharacterMap)
        {
            gameCode = gameCode.Replace(unambiguousChar, ambiguousChar);
        }

        return int.Parse(gameCode, NumberStyles.HexNumber);
    }

    public static string RandomSeedToGameCode(int randomSeed)
    {
        var gameCode = randomSeed.ToString("X4");

        // Swap ambiguous hex characters with unambiguous ones.
        foreach (var (ambiguousChar, unambiguousChar) in ambiguousCharacterMap)
        {
            gameCode = gameCode.Replace(ambiguousChar, unambiguousChar);
        }

        return gameCode;
    }
}
