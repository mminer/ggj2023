using System.Collections;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameState gameState;
    [SerializeField] GameEvent gameOverEvent;
    [SerializeField] float moveDurationSeconds = 1;

    public Vector3Int position => Vector3Int.RoundToInt(transform.position);

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        FaceHero();
    }

    public IEnumerator MoveTowardsOrAttackHero()
    {
        if (!gameState.dungeon.TryGetPath(position, gameState.hero.position, out var path))
        {
            Debug.LogWarning("No path from enemy to player. Should this be possible?");
            yield return null;
        }

        var newPosition = path.First();

        if (newPosition == gameState.hero.position)
        {
            yield return AttemptAttack();
        }
        else
        {
            animator.SetBool(CharacterAnimatorID.isMoving, true);
            yield return transform.MoveToPosition(newPosition, moveDurationSeconds);
            animator.SetBool(CharacterAnimatorID.isMoving, false);
        }

        yield return new WaitForSeconds(1);
    }

    public void Die()
    {
        IEnumerator DieRoutine()
        {
            yield return new WaitForSeconds(1);
            gameState.enemies.Remove(this);
            Destroy(gameObject);
        }

        Debug.Log("Killing enemy.");
        animator.SetTrigger(CharacterAnimatorID.die);
        StartCoroutine(DieRoutine());
    }

    IEnumerator AttemptAttack()
    {
        // Failed attack
        if (gameState.hero.isDefending)
        {
            // TODO: show failed attack effects
            gameState.hero.BlockAttack();
        }
        // Successful attack
        else
        {
            animator.SetTrigger(CharacterAnimatorID.enemyAttackSuccess);
            gameOverEvent.Invoke();
        }

        yield return new WaitForSeconds(1);
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
