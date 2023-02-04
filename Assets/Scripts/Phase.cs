using System;

/// <summary>
/// Game stage.
/// </summary>
public enum Phase
{
    DrawCards,
    CreateQueue,
    PlayCards,
}

public static class PhaseExtensions
{
    public static Phase GetNextPhase(this Phase phase)
    {
        return phase switch
        {
            Phase.DrawCards => Phase.CreateQueue,
            Phase.CreateQueue => Phase.PlayCards,
            Phase.PlayCards => Phase.CreateQueue,
            _ => throw new ArgumentOutOfRangeException(nameof(phase), phase, null),
        };
    }
}
