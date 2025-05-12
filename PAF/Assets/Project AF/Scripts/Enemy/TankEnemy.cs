using UnityEngine;

/// <summary>
/// 탱커 적 클래스
/// </summary>
public class TankEnemy : BaseEnemy
{
    [Header("Tank Skill Settings")]
    [Tooltip("철벽 스킬이 활성화되는 체력 비율 (0-1)")]
    public float ironWallThreshold = 0.5f;

    [Tooltip("철벽 스킬 활성화시 받는 데미지 감소율 (0-1)")]
    public float damageReduction = 0.5f;

    [Header("Iron Wall Effect")]
    [Tooltip("철벽 스킬 이펙트 애니메이션")]
    public RuntimeAnimatorController ironWallEffectAnimator;
    
    [Tooltip("이펙트 크기")]
    public Vector2 effectSize = new Vector2(2f, 2f);

    private bool isIronWallActive = false;
    private GameObject ironWallEffect = null;

    protected override void Awake()
    {
        base.Awake();
        if (maxHealth == 0) maxHealth = 150f;  // 기본 체력
    }

    public override void TakeDamage(float damage)
    {
        // 철벽 스킬이 활성화되어 있으면 데미지 감소
        if (isIronWallActive)
        {
            damage *= (1 - damageReduction);
            Debug.Log($"철벽 스킬 발동! 데미지 {damageReduction * 100}% 감소");
        }

        base.TakeDamage(damage);

        // 체력이 임계값 이하로 떨어졌을 때 철벽 스킬 활성화
        float healthRatio = currentHealth / maxHealth;
        if (!isIronWallActive && healthRatio <= ironWallThreshold)
        {
            ActivateIronWall();
        }
    }

    protected override void Die()
    {
        // 사망 시 이펙트 제거
        if (ironWallEffect != null)
        {
            Destroy(ironWallEffect);
            ironWallEffect = null;
        }
        base.Die();
    }

    private void ActivateIronWall()
    {
        isIronWallActive = true;
        Debug.Log("철벽 스킬이 활성화되었습니다!");

        // 시각적 효과 생성
        if (ironWallEffectAnimator != null && ironWallEffect == null)
        {
            ironWallEffect = new GameObject("IronWallEffect");
            ironWallEffect.transform.position = transform.position;
            
            // 스프라이트 렌더러 추가
            SpriteRenderer spriteRenderer = ironWallEffect.AddComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 1; // 적보다 위에 표시
            
            // 애니메이터 추가
            Animator animator = ironWallEffect.AddComponent<Animator>();
            animator.runtimeAnimatorController = ironWallEffectAnimator;
            
            // 크기 설정
            ironWallEffect.transform.localScale = effectSize;
            
            // 이펙트를 적에 부모로 설정
            ironWallEffect.transform.parent = transform;
        }
    }
} 