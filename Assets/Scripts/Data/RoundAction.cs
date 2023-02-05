public interface IRoundAction
{
    public int playerIndex { get; }
}

public readonly struct RoundAction_Discard : IRoundAction
{
    public int playerIndex { get; }
    public readonly Card[] cards;

    public RoundAction_Discard(int playerIndex, Card[] cards)
    {
        this.playerIndex = playerIndex;
        this.cards = cards;
    }
}

public readonly struct RoundAction_SubmitQueue : IRoundAction
{
    public int playerIndex { get; }
    public readonly Card[] cards;

    public RoundAction_SubmitQueue(int playerIndex, Card[] cards)
    {
        this.playerIndex = playerIndex;
        this.cards = cards;
    }
}
