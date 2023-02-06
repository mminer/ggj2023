using UnityEngine;

public static class CharacterAnimatorID
{
    public static readonly int blockAttack = Animator.StringToHash("blockAttack");
    public static readonly int die = Animator.StringToHash("die");
    public static readonly int enemyAttackFailure = Animator.StringToHash("enemyAttackFailure");
    public static readonly int enemyAttackSuccess = Animator.StringToHash("enemyAttackSuccess");
    public static readonly int heroAttack = Animator.StringToHash("heroAttack");
    public static readonly int isMoving = Animator.StringToHash("isMoving");
}
