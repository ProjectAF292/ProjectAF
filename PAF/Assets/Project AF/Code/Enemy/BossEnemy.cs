using UnityEngine;

/// <summary>
/// 보스 적의 기본 속성을 정의하는 클래스
/// </summary>
public class BossEnemy : BaseEnemy
{
    protected override void Awake()
    {
        base.Awake();
        if (maxHealth == 0) maxHealth = 500;  // 기본 체력
    }
} 