using System.Collections.Generic;

public struct Player
{
    public readonly int index;
    public readonly List<Card> hand;

    public Player(int index)
    {
        this.index = index;
        this.hand = new List<Card>();
    }
}
