using UnityEngine;

/// <summary>
/// 탱크 적의 AI를 처리하는 클래스
/// </summary>
public class TankEnemyAI : BaseEnemyAI
{
    private TankEnemyAttack _attackComponent;
    private TankEnemy _tankEnemy;

    protected override void Awake()
    {
        base.Awake();
        _attackComponent = GetComponent<TankEnemyAttack>();
        _tankEnemy = GetComponent<TankEnemy>();
        
        if (_attackComponent == null)
        {
            Debug.LogError("TankEnemyAttack component not found!");
        }
        
        if (_tankEnemy == null)
        {
            Debug.LogError("TankEnemy component not found!");
        }
    }

    protected override void HandleAIBehavior()
    {
        // AI 행동 로직 구현 예정
    }
} 