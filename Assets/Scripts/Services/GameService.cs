using UnityEngine;

public class GameService : Services.Service
{
    [Header("Events")]
    [SerializeField] GameEvent gameOverEvent;

    public void EndGame()
    {
        gameOverEvent.Invoke();
    }
}
