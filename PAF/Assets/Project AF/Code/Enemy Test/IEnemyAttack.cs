using UnityEngine;

public interface IEnemyAttack
{
    void Initialize(EnemyData data);
    void Attack(Transform target);
    bool CanAttack(Transform target);
    void OnAttackComplete();
} 