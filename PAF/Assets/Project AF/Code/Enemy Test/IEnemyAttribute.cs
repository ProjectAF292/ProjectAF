using UnityEngine;

public interface IEnemyAttribute
{
    void Initialize(EnemyData data);
    void ApplyAttributeEffect(GameObject target);
    ElementType GetElementType();
} 