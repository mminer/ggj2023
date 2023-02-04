using System;
using UnityEngine;

public class GameService : Services.Service
{
    [Header("Events")]
    [SerializeField] GameEvent gameOverEvent;

    public Phase currentPhase { get; private set; }

    public void EndGame()
    {
        gameOverEvent.Invoke();
    }

    public void NextPhase()
    {
        currentPhase = currentPhase switch
        {
            Phase.DrawCards => Phase.CreateQueue,
            Phase.CreateQueue => Phase.PlayCards,
            Phase.PlayCards => Phase.CreateQueue,
            _ => throw new ArgumentOutOfRangeException(nameof(currentPhase), currentPhase, null),
        };
    }
}
