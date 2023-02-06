using System.Collections.Generic;

public struct Player
{
    public readonly string name;
    public readonly int index;
    public readonly List<Card> hand;

    public Player(int index, string name="Roots")
    {
        this.index = index;
        this.name = name;
        this.hand = new List<Card>();
    }
}
