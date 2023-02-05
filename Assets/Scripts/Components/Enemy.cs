using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameState gameState;

    void Update()
    {
        FaceHero();
    }

    public void MoveTowardsHero()
    {
        var position = Vector3Int.RoundToInt(transform.position);

        if (!gameState.dungeon.TryGetPath(position, gameState.hero.position, out var path))
        {
            Debug.LogWarning("No path from enemy to player. Should this be possible?");
            return;
        }

        var newPosition = path.First();

        if (newPosition == gameState.hero.position)
        {
            return;
        }

        transform.position = newPosition;
    }

    void FaceHero()
    {
        if (gameState.hero == null)
        {
            return;
        }

        transform.LookAt(gameState.hero.transform);
    }

}
