using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] GameState gameState;
    [SerializeField] ParticleSystem attackFX;
    [SerializeField] float moveDurationSeconds = 1;

    public bool isDefending;
    public Vector3Int position => Vector3Int.RoundToInt(transform.position);

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public IEnumerator ApplyCard(Card card)
    {
        Debug.Log($"Applying card: {card}");

        switch (card)
        {
            case Card.Attack:
                yield return Attack();
                break;

            case Card.Defend:
                isDefending = true;
                break;

            case Card.DoNothing:
                break;

            case Card.MoveEast:
                yield return Move(Vector3Int.right);
                break;

            case Card.MoveNorth:
                yield return Move(Vector3Int.forward);
                break;

            case Card.MoveRandom:
                var randomDirection = MiscUtility.GetRandomDirection(gameState.rng);
                Debug.Log($"Result of random direction card: {randomDirection}");
                yield return Move(randomDirection);
                break;

            case Card.MoveSouth:
                yield return Move(Vector3Int.back);
                break;

            case Card.MoveWest:
                yield return Move(Vector3Int.left);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(card), card, null);
        }

        yield break;
    }

    public void BlockAttack()
    {
        animator.SetTrigger(CharacterAnimatorID.blockAttack);
    }

    IEnumerator Attack()
    {
        isDefending = false;
        animator.SetTrigger(CharacterAnimatorID.heroAttack);
        attackFX.Play();

        /*
        foreach (var adjacentPosition in gameState.dungeon.GetAdjacentPositions(position, true))
        {
            var enemy = gameState.enemies.FirstOrDefault(enemy => enemy.position == adjacentPosition);

            if (enemy != null)
            {
                enemy.Die();
            }
        }
        */

        yield return new WaitForSeconds(1);
    }

    IEnumerator Move(Vector3Int direction)
    {
        isDefending = false;

        var newPosition = Vector3Int.RoundToInt(transform.position) + direction;
        transform.LookAt(newPosition);

        if (gameState.dungeon.IsWalkable(newPosition))
        {
            animator.SetBool(CharacterAnimatorID.isMoving, true);
            yield return transform.MoveToPosition(newPosition, moveDurationSeconds);
            animator.SetBool(CharacterAnimatorID.isMoving, false);
        }
        else
        {
            // TODO: play animation to show that player is against a wall
            yield return new WaitForSeconds(1);
        }
    }
}
