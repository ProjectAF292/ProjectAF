using UnityEngine;

/// <summary>
/// 탱크 적의 공격을 처리하는 클래스
/// </summary>
public class TankEnemyAttack : BaseEnemyAttack
{
    private TankEnemy _tankEnemy;

    protected override void Awake()
    {
        base.Awake();
        _tankEnemy = GetComponent<TankEnemy>();
        
        // 기본값 설정
        if (damage == 0) damage = 35;
        if (attackCooldown == 0) attackCooldown = 2.5f;
        if (attackRange == 0) attackRange = 2f;
    }

    protected override void ExecuteAttack()
    {
        // 공격 로직 구현 예정
    }
} 