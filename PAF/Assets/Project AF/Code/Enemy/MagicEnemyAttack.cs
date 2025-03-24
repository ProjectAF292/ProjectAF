using UnityEngine;

/// <summary>
/// 마법사 적의 공격을 처리하는 클래스
/// </summary>
public class MagicEnemyAttack : BaseEnemyAttack
{
    private MagicEnemy _magicEnemy;

    protected override void Awake()
    {
        base.Awake();
        _magicEnemy = GetComponent<MagicEnemy>();
        
        // 기본값 설정
        if (damage == 0) damage = 20;
        if (attackCooldown == 0) attackCooldown = 1.5f;
        if (attackRange == 0) attackRange = 5f;
    }

    protected override void ExecuteAttack()
    {
        // 공격 로직 구현 예정
    }
} 