using UnityEngine;

/// <summary>
/// 근접 공격 적의 기본 속성을 정의하는 클래스
/// </summary>
public class MeleeEnemy : BaseEnemy
{
    protected override void Awake()
    {
        base.Awake();
        if (maxHealth == 0) maxHealth = 100;  // 기본 체력
    }
} 