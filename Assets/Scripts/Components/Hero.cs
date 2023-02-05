using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] GameState gameState;

    public bool isDefending;
    public Vector3Int position => Vector3Int.RoundToInt(transform.position);

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ApplyCard(Card card)
    {
        Debug.Log($"Applying card: {card}");

        switch (card)
        {
            case Card.Attack:
                Attack();
                break;

            case Card.Defend:
                isDefending = true;
                break;

            case Card.DoNothing:
                break;

            case Card.MoveEast:
                Move(Vector3Int.right);
                break;

            case Card.MoveNorth:
                Move(Vector3Int.forward);
                break;

            case Card.MoveRandom:
                var randomDirection = MiscUtility.GetRandomDirection(gameState.rng);
                Debug.Log($"Result of random direction card: {randomDirection}");
                Move(randomDirection);
                break;

            case Card.MoveSouth:
                Move(Vector3Int.back);
                break;

            case Card.MoveWest:
                Move(Vector3Int.left);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(card), card, null);
        }
    }

    public void BlockAttack()
    {
        animator.SetTrigger(CharacterAnimatorID.blockAttack);
    }

    void Attack()
    {
        animator.SetTrigger(CharacterAnimatorID.heroAttack);

        foreach (var adjacentPosition in gameState.dungeon.GetAdjacentPositions(position, true))
        {
            var enemy = gameState.enemies.FirstOrDefault(enemy => enemy.position == adjacentPosition);

            if (enemy != null)
            {
                enemy.Die();
            }
        }

        isDefending = false;
    }

    void Move(Vector3Int direction)
    {
        var newPosition = Vector3Int.RoundToInt(transform.position) + direction;

        if (!gameState.dungeon.IsWalkable(newPosition))
        {
            return;
        }

        transform.position = newPosition;
        isDefending = false;
    }
}
