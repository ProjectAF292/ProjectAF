using UnityEngine;

/// <summary>
/// 엘리트 적의 기본 속성을 정의하는 클래스
/// </summary>
public class EliteEnemy : BaseEnemy
{
    protected override void Awake()
    {
        base.Awake();
        if (maxHealth == 0) maxHealth = 150;  // 기본 체력
    }
} 