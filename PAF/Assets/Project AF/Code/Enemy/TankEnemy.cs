using UnityEngine;

/// <summary>
/// 탱크 적의 기본 속성을 정의하는 클래스
/// </summary>
public class TankEnemy : BaseEnemy
{
    protected override void Awake()
    {
        base.Awake();
        if (maxHealth == 0) maxHealth = 200;  // 기본 체력
    }
} 