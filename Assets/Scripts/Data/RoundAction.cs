public interface IRoundAction
{
    public int playerIndex { get; }

    public readonly struct Discard : IRoundAction
    {
        public int playerIndex { get; }
        public readonly Card card;

        public Discard(int playerIndex, Card card)
        {
            this.playerIndex = playerIndex;
            this.card = card;
        }
    }

    public readonly struct SubmitQueue : IRoundAction
    {
        public int playerIndex { get; }
        public readonly Card[] queue;

        public SubmitQueue(int playerIndex, Card[] queue)
        {
            this.playerIndex = playerIndex;
            this.queue = queue;
        }
    }
}
