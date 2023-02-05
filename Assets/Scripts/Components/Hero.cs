using System;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public GameState gameState { private get; set; }
    public Vector3Int position => Vector3Int.RoundToInt(transform.position);

    public void ApplyCard(Card card)
    {
        Debug.Log($"Applying card: {card}");

        switch (card)
        {
            case Card.DoNothing:
                break;

            case Card.MoveEast:
                MoveInDirection(Vector3Int.right);
                break;

            case Card.MoveNorth:
                MoveInDirection(Vector3Int.forward);
                break;

            case Card.MoveRandom:
                var randomDirection = MiscUtility.GetRandomDirection(gameState.rng);
                MoveInDirection(randomDirection);
                break;

            case Card.MoveSouth:
                MoveInDirection(Vector3Int.back);
                break;

            case Card.MoveWest:
                MoveInDirection(Vector3Int.left);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(card), card, null);
        }
    }

    void MoveInDirection(Vector3Int direction)
    {
        var nextPosition = Vector3Int.RoundToInt(transform.position) + direction;

        if (!gameState.dungeon[nextPosition].IsWalkable)
        {
            return;
        }

        transform.position = nextPosition;
    }
}
