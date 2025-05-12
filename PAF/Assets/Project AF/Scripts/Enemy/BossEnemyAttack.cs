using UnityEngine;

/// <summary>
/// 보스 적의 공격을 처리하는 클래스
/// </summary>
public class BossEnemyAttack : BaseEnemyAttack
{
    private BossEnemy _bossEnemy;

    protected override void Awake()
    {
        base.Awake();
        _bossEnemy = GetComponent<BossEnemy>();
        
        // 기본값 설정
        if (damage == 0) damage = 30;
        if (attackCooldown == 0) attackCooldown = 2f;
        if (attackRange == 0) attackRange = 3f;
    }

    protected override void ExecuteAttack()
    {
        // 공격 로직 구현 예정
    }
} 