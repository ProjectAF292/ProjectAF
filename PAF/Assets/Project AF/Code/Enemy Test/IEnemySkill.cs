using UnityEngine;

public interface IEnemySkill
{
    void Initialize(EnemyData data);
    void UseSkill(Transform target);
    bool CanUseSkill();
    void OnSkillComplete();
} 