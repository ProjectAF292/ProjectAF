using UnityEngine;
using System.Collections;

/// <summary>
/// 근접 적의 공격을 처리하는 클래스
/// </summary>
public class MeleeEnemyAttack : BaseEnemyAttack
{
    [Header("Melee Attack Settings")]
    [Tooltip("공격 범위의 크기")]
    public Vector2 attackSize = new Vector2(2f, 2f);  // 범위를 좀 더 크게 조정
    
    [Tooltip("공격 범위의 오프셋")]
    public Vector2 attackOffset = new Vector2(1f, 0f);  // 오프셋도 조정

    [Header("Attack Effect")]
    [Tooltip("공격 이펙트 애니메이션")]
    public RuntimeAnimatorController attackEffectAnimator;
    
    [Tooltip("이펙트 크기")]
    public Vector2 effectSize = Vector2.one;

    private MeleeEnemy _meleeEnemy;
    private GameObject _hitBox;
    private BoxCollider2D _hitBoxCollider;

    protected override void Awake()
    {
        base.Awake();
        _meleeEnemy = GetComponent<MeleeEnemy>();
        
        // 기본값 설정
        if (damage == 0) damage = 15;
        if (attackCooldown == 0) attackCooldown = 1.2f;
        if (attackRange == 0) attackRange = 2f;  // 공격 범위도 조정

        // 히트박스 생성
        CreateHitBox();
    }

    private void CreateHitBox()
    {
        _hitBox = new GameObject("MeleeHitBox");
        _hitBox.transform.parent = transform;
        
        // Rigidbody2D 추가
        Rigidbody2D rb = _hitBox.AddComponent<Rigidbody2D>();
        rb.isKinematic = true;  // 물리 영향을 받지 않도록 설정
        rb.simulated = true;    // 트리거 감지를 위해 시뮬레이션은 활성화
        
        // 박스 콜라이더 추가
        _hitBoxCollider = _hitBox.AddComponent<BoxCollider2D>();
        _hitBoxCollider.size = attackSize;
        _hitBoxCollider.isTrigger = true;
        
        // 히트박스 스크립트 추가
        MeleeHitBox hitBoxScript = _hitBox.AddComponent<MeleeHitBox>();
        hitBoxScript.SetDamage(damage);  // SetDamage 메서드 사용
        
        // 초기 위치 설정
        _hitBox.transform.localPosition = Vector3.zero;
        
        // 히트박스 비활성화
        _hitBox.SetActive(false);
    }

    private void OnValidate()
    {
        // 인스펙터에서 값이 변경될 때 히트박스 크기와 데미지 업데이트
        if (_hitBoxCollider != null)
        {
            _hitBoxCollider.size = attackSize;
        }
        
        // 데미지 값이 변경되면 히트박스의 데미지도 업데이트
        if (_hitBox != null)
        {
            MeleeHitBox hitBoxScript = _hitBox.GetComponent<MeleeHitBox>();
            if (hitBoxScript != null)
            {
                hitBoxScript.SetDamage(damage);
            }
        }
    }

    protected override void ExecuteAttack()
    {
        if (target == null) return;

        // 공격 방향 계산
        Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
        
        // 공격 범위 위치 계산
        Vector2 attackPosition = (Vector2)transform.position + direction * attackOffset.x;
        
        // 히트박스 위치 및 회전 설정
        _hitBox.transform.position = attackPosition;
        
        // 히트박스 크기 업데이트 (실시간으로 크기가 변경될 수 있으므로)
        if (_hitBoxCollider != null)
        {
            _hitBoxCollider.size = attackSize;
        }
        
        // 공격 이펙트 생성 및 히트박스 활성화
        _hitBox.SetActive(true);  // 히트박스 활성화
        
        if (attackEffectAnimator != null)
        {
            GameObject effect = new GameObject("AttackEffect");
            effect.transform.position = attackPosition;
            
            // 스프라이트 렌더러 추가
            SpriteRenderer spriteRenderer = effect.AddComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 1; // 적보다 위에 표시
            
            // 애니메이터 추가
            Animator animator = effect.AddComponent<Animator>();
            animator.runtimeAnimatorController = attackEffectAnimator;
            
            // 크기 설정
            effect.transform.localScale = effectSize;
            
            // 방향에 따라 회전
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            effect.transform.rotation = Quaternion.Euler(0, 0, angle);
            
            // 이펙트와 히트박스 함께 제거
            StartCoroutine(DeactivateAfterEffect(0.5f));
            
            // 애니메이션 종료 후 제거
            Destroy(effect, 0.5f);
        }
    }

    private IEnumerator DeactivateAfterEffect(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (_hitBox != null)
        {
            _hitBox.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        // 기본 공격 범위 시각화 (원형)
        Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // 근접 공격 범위 시각화
        Vector2 direction;
        if (target != null)
        {
            direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
        }
        else
        {
            // 타겟이 없을 때는 오른쪽을 기본 방향으로 사용
            direction = Vector2.right;
        }
        
        Vector2 attackPosition = (Vector2)transform.position + direction * attackOffset.x;
        
        // 공격 범위 박스 표시
        Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
        Gizmos.DrawWireCube(attackPosition, attackSize);
        Gizmos.color = new Color(1f, 1f, 0f, 0.1f);
        Gizmos.DrawCube(attackPosition, attackSize);
        
        // 공격 오프셋 라인 표시
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(attackPosition.x, attackPosition.y, 0));
        
        // 중심점 표시
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(attackPosition, 0.1f);
    }
} 