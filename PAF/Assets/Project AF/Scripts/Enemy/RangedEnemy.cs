using UnityEngine;

/// <summary>
/// 원거리 적 클래스
/// </summary>
public class RangedEnemy : BaseEnemy
{
    protected override void Awake()
    {
        base.Awake();
        if (maxHealth == 0) maxHealth = 80f;  // 기본 체력
    }
} 