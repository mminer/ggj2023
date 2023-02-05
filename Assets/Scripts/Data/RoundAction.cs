using System.Collections.Generic;
using System.Linq;

public interface IRoundAction
{
    public int playerIndex { get; }
}

public readonly struct RoundAction_Discard : IRoundAction
{
    public int playerIndex { get; }
    public readonly Card[] cards;

    public RoundAction_Discard(int playerIndex, IEnumerable<Card> cards)
    {
        this.playerIndex = playerIndex;
        this.cards = cards.ToArray();
    }

    public override string ToString()
    {
        return $"RoundAction_Discard; playerIndex: {playerIndex}";
    }
}

public readonly struct RoundAction_SubmitQueue : IRoundAction
{
    public int playerIndex { get; }
    public readonly Card[] cards;

    public RoundAction_SubmitQueue(int playerIndex, IEnumerable<Card> cards)
    {
        this.playerIndex = playerIndex;
        this.cards = cards.ToArray();
    }

    public override string ToString()
    {
        return $"RoundAction_SubmitQueue; playerIndex: {playerIndex}";
    }
}
