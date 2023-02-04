using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    static readonly Vector3Int[] directions =
    {
        Vector3Int.back,
        Vector3Int.forward,
        Vector3Int.left,
        Vector3Int.right,
    };

    public static List<Card> DeckFromConfig(Rules rules)
    {
        var deck = new List<Card>();

        foreach (var cardConfig in rules.deckConfig)
        {
            for (var i = 0; i < cardConfig.count; i++)
            {
                deck.Add(cardConfig.card);
            }
        }

        deck.Shuffle();
        return deck;
    }

    public static Vector3Int GetRandomDirection()
    {
        return directions[Random.Range(0, 4)];
    }

}
