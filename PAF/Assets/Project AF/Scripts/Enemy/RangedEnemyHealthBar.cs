using UnityEngine;

/// <summary>
/// 원거리 적의 체력바 클래스
/// </summary>
public class RangedEnemyHealthBar : BaseEnemyHealthBar
{
    protected override void Awake()
    {
        base.Awake();
        // 원거리 적의 체력바는 기본 설정을 사용
    }
} 