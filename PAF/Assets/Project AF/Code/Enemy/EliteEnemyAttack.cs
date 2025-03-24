using UnityEngine;

/// <summary>
/// 엘리트 적의 공격을 처리하는 클래스
/// </summary>
public class EliteEnemyAttack : BaseEnemyAttack
{
    private EliteEnemy _eliteEnemy;

    protected override void Awake()
    {
        base.Awake();
        _eliteEnemy = GetComponent<EliteEnemy>();
        
        // 기본값 설정
        if (damage == 0) damage = 25;
        if (attackCooldown == 0) attackCooldown = 1.8f;
        if (attackRange == 0) attackRange = 2.5f;
    }

    protected override void ExecuteAttack()
    {
        // 공격 로직 구현 예정
    }
} 