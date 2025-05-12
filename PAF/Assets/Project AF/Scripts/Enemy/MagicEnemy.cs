using UnityEngine;

/// <summary>
/// 마법사 적의 기본 속성을 정의하는 클래스
/// </summary>
public class MagicEnemy : BaseEnemy
{
    protected override void Awake()
    {
        base.Awake();
        if (maxHealth == 0) maxHealth = 80;  // 기본 체력
    }
} 