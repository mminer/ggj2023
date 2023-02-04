using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class Rules : ScriptableObject
{
    [Serializable]
    public struct CardConfig
    {
        public Card card;
        public int count;
    }

    public const int maxRandomSeed = 65_536; // 16^4 so that all seeds can be represented by 4 character hex string

    public int enemyCount = 4;

    [Header("Cards")]
    public int drawCount = 1;
    public int handSize = 5;
    public int queueSize = 3;

    [Space]
    public CardConfig[] deckConfig =
    {
        new() { card = Card.DoNothing, count = 2 },
        new() { card = Card.MoveEast, count = 2 },
        new() { card = Card.MoveNorth, count = 2 },
        new() { card = Card.MoveRandom, count = 1 },
        new() { card = Card.MoveSouth, count = 2 },
        new() { card = Card.MoveWest, count = 2 },
    };

    public int deckSize => deckConfig.Sum(cardConfig => cardConfig.count);

    [Header("Map")]
    public int mapWidth = 32;
    public int mapHeight = 32;
    public int maxRooms = 30;
    public int roomMaxSize = 10;
    public int roomMinSize = 3;
}
