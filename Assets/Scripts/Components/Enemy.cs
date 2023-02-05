using System.Collections;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameState gameState;
    [SerializeField] GameEvent gameOverEvent;

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

    public void MoveTowardsOrAttackHero()
    {
        if (!gameState.dungeon.TryGetPath(position, gameState.hero.position, out var path))
        {
            Debug.LogWarning("No path from enemy to player. Should this be possible?");
            return;
        }

        var newPosition = path.First();

        if (newPosition == gameState.hero.position)
        {
            AttemptAttack();
        }
        else
        {
            transform.position = newPosition;
        }
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

    void AttemptAttack()
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
