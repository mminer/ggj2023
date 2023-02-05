using UnityEngine;

public static class CharacterAnimatorID
{
    public static readonly int blockAttack = Animator.StringToHash("BlockAttack");
    public static readonly int die = Animator.StringToHash("Die");
    public static readonly int enemyAttackFailure = Animator.StringToHash("EnemyAttackFailure");
    public static readonly int enemyAttackSuccess = Animator.StringToHash("EnemyAttackSuccess");
    public static readonly int heroAttack = Animator.StringToHash("HeroAttack");
}
