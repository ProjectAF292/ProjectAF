using UnityEngine;

/// <summary>
/// 엘리트 적의 AI를 처리하는 클래스
/// </summary>
public class EliteEnemyAI : BaseEnemyAI
{
    private EliteEnemyAttack _attackComponent;
    private EliteEnemy _eliteEnemy;

    protected override void Awake()
    {
        base.Awake();
        _attackComponent = GetComponent<EliteEnemyAttack>();
        _eliteEnemy = GetComponent<EliteEnemy>();
        
        if (_attackComponent == null)
        {
            Debug.LogError("EliteEnemyAttack component not found!");
        }
        
        if (_eliteEnemy == null)
        {
            Debug.LogError("EliteEnemy component not found!");
        }
    }

    protected override void HandleAIBehavior()
    {
        // AI 행동 로직 구현 예정
    }
} 