using UnityEngine;

/// <summary>
/// 보스 적의 AI를 처리하는 클래스
/// </summary>
public class BossEnemyAI : BaseEnemyAI
{
    private BossEnemyAttack _attackComponent;
    private BossEnemy _bossEnemy;

    protected override void Awake()
    {
        base.Awake();
        _attackComponent = GetComponent<BossEnemyAttack>();
        _bossEnemy = GetComponent<BossEnemy>();
        
        if (_attackComponent == null)
        {
            Debug.LogError("BossEnemyAttack component not found!");
        }
        
        if (_bossEnemy == null)
        {
            Debug.LogError("BossEnemy component not found!");
        }
    }

    protected override void HandleAIBehavior()
    {
        // AI 행동 로직 구현 예정
    }
} 