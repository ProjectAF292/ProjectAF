using UnityEngine;

/// <summary>
/// 마법사 적의 AI를 처리하는 클래스
/// </summary>
public class MagicEnemyAI : BaseEnemyAI
{
    private MagicEnemyAttack _attackComponent;
    private MagicEnemy _magicEnemy;

    protected override void Awake()
    {
        base.Awake();
        _attackComponent = GetComponent<MagicEnemyAttack>();
        _magicEnemy = GetComponent<MagicEnemy>();
        
        if (_attackComponent == null)
        {
            Debug.LogError("MagicEnemyAttack component not found!");
        }
        
        if (_magicEnemy == null)
        {
            Debug.LogError("MagicEnemy component not found!");
        }
    }

    protected override void HandleAIBehavior()
    {
        // AI 행동 로직 구현 예정
    }
} 