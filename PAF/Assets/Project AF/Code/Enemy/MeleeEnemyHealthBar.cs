using UnityEngine;

/// <summary>
/// 근접 적의 체력바 클래스
/// </summary>
public class MeleeEnemyHealthBar : BaseEnemyHealthBar
{
    protected override void Awake()
    {
        base.Awake();
        // 근접 적의 체력바는 기본 설정을 사용
    }
} 