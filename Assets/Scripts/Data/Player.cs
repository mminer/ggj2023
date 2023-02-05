using System.Collections.Generic;

public struct Player
{
    public readonly int id;
    public readonly List<Card> hand;
    public readonly List<Card> queue;

    public Player(int id)
    {
        this.id = id;
        this.hand = new List<Card>();
        this.queue = new List<Card>();
    }
}
