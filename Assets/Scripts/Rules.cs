using UnityEngine;

[CreateAssetMenu]
public class Rules : ScriptableObject
{
    public int enemyCount = 4;

    [Header("Cards")]
    public int deckCount = 30;
    public int discardableCount = 1;
    public int handCount = 5;
    public int queueCount = 3;

    [Header("Map")]
    public int mapWidth = 32;
    public int mapHeight = 32;
    public int maxRooms = 30;
    public int roomMaxSize = 10;
    public int roomMinSize = 3;
}
