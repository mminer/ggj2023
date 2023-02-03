using System;
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

    public int enemyCount = 4;

    [Header("Cards")]
    public int allowedDiscards = 1;
    public int handSize = 5;
    public int queueSize = 3;

    [Space]
    public CardConfig[] deck =
    {
        new() { card = Card.DoNothing, count = 2 },
        new() { card = Card.MoveEast, count = 2 },
        new() { card = Card.MoveNorth, count = 2 },
        new() { card = Card.MoveRandom, count = 1 },
        new() { card = Card.MoveSouth, count = 2 },
        new() { card = Card.MoveWest, count = 2 },
    };

    [Header("Map")]
    public int mapWidth = 32;
    public int mapHeight = 32;
    public int maxRooms = 30;
    public int roomMaxSize = 10;
    public int roomMinSize = 3;
}
