using UnityEngine;

public class FireAttribute : MonoBehaviour, IEnemyAttribute
{
    private EnemyData enemyData;

    public void Initialize(EnemyData data)
    {
        enemyData = data;
    }

    public void ApplyAttributeEffect(GameObject target)
    {
        // 속성 효과는 나중에 구현
    }

    public ElementType GetElementType()
    {
        return ElementType.Fire;
    }
} 