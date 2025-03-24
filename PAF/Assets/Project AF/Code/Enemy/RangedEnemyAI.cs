using UnityEngine;

/// <summary>
/// 원거리 공격 적의 AI를 처리하는 클래스
/// </summary>
public class RangedEnemyAI : BaseEnemyAI
{
    private RangedEnemyAttack _attackComponent;
    private bool _isPlayerInRange;

    [System.Serializable]
    public class RangedSettings
    {
        [Header("Movement Settings")]
        [Tooltip("원거리 적이 플레이어를 감지하는 범위")]
        public float detectionRange = 8f;

        [Tooltip("원거리 적이 플레이어를 공격할 수 있는 범위")]
        public float attackRange = 6f;

        [Tooltip("원거리 적의 이동 속도")]
        public float moveSpeed = 1.5f;

        [Tooltip("원거리 적이 플레이어와 유지하려는 최소 거리")]
        public float minimumRange = 4f;
    }

    [Header("Ranged Enemy Settings")]
    [SerializeField]
    private RangedSettings rangedSettings = new RangedSettings();

    protected override void Awake()
    {
        base.Awake();
        _attackComponent = GetComponent<RangedEnemyAttack>();
        
        if (_attackComponent == null)
        {
            Debug.LogError("RangedEnemyAttack component not found!");
        }
    }

    protected override void HandleAIBehavior()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);
        Vector2 directionToPlayer = ((Vector2)_player.position - (Vector2)transform.position).normalized;
        
        _isPlayerInRange = distanceToPlayer <= rangedSettings.detectionRange;
        
        if (_isPlayerInRange)
        {
            // 공격 범위 내에 있고 최소 거리보다 멀리 있으면 공격
            if (distanceToPlayer <= rangedSettings.attackRange && distanceToPlayer >= rangedSettings.minimumRange)
            {
                _rb.velocity = Vector2.zero;
                UpdateDirection(directionToPlayer);
                _attackComponent.TryAttack();
            }
            // 최소 거리보다 가까이 있으면 도망
            else if (distanceToPlayer < rangedSettings.minimumRange)
            {
                _rb.velocity = -directionToPlayer * rangedSettings.moveSpeed;
                UpdateAnimation(-directionToPlayer);
            }
            // 공격 범위 밖에 있으면 플레이어를 추격
            else
            {
                _rb.velocity = directionToPlayer * rangedSettings.moveSpeed;
                UpdateAnimation(directionToPlayer);
            }
        }
        else
        {
            HandleIdle();
        }
    }

    /// <summary>
    /// 대기 상태 처리
    /// </summary>
    private void HandleIdle()
    {
        _rb.velocity = Vector2.zero;
        
        // 기본 방향을 아래쪽(Front)으로 설정
        _lastDirection = Vector2.down;
        
        if (_animator != null)
        {
            _animator.SetBool("isMoving", false);
            _animator.SetFloat("moveX", 0);
            _animator.SetFloat("moveY", -1);  // Front를 위해 -1 설정
            if (_spriteRenderer != null)
            {
                _spriteRenderer.flipX = false;
            }
        }
    }

    /// <summary>
    /// 애니메이션 업데이트
    /// </summary>
    private void UpdateAnimation(Vector2 direction)
    {
        if (_animator == null) return;
        if (_enemy != null && _enemy.IsDead()) return;

        bool isMoving = _rb.velocity.magnitude > 0.01f;
        _animator.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            UpdateDirection(direction);
        }
    }

    /// <summary>
    /// 방향 업데이트
    /// </summary>
    protected override void UpdateDirection(Vector2 direction)
    {
        if (_animator == null) return;

        float angle = Vector2.SignedAngle(Vector2.right, direction);
        if (angle < 0) angle += 360f;
        
        if (angle >= 315f || angle < 45f) // 오른쪽
        {
            _animator.SetFloat("moveX", 1);
            _animator.SetFloat("moveY", 0);
            if (_spriteRenderer != null) _spriteRenderer.flipX = false;
        }
        else if (angle >= 45f && angle < 135f) // 위
        {
            _animator.SetFloat("moveX", 0);
            _animator.SetFloat("moveY", 1);
            if (_spriteRenderer != null) _spriteRenderer.flipX = false;
        }
        else if (angle >= 135f && angle < 225f) // 왼쪽
        {
            _animator.SetFloat("moveX", -1);
            _animator.SetFloat("moveY", 0);
            if (_spriteRenderer != null) _spriteRenderer.flipX = true;
        }
        else // 아래 (225f ~ 315f)
        {
            _animator.SetFloat("moveX", 0);
            _animator.SetFloat("moveY", -1);
            if (_spriteRenderer != null) _spriteRenderer.flipX = false;
        }
        
        _lastDirection = new Vector2(_animator.GetFloat("moveX"), _animator.GetFloat("moveY"));
    }

    private void OnDrawGizmos()
    {
        // 감지 범위 시각화 (빨간색)
        Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
        Gizmos.DrawWireSphere(transform.position, rangedSettings.detectionRange);

        // 공격 범위 시각화 (노란색)
        Gizmos.color = new Color(1f, 1f, 0f, 0.2f);
        Gizmos.DrawWireSphere(transform.position, rangedSettings.attackRange);

        // 최소 거리 시각화 (파란색)
        Gizmos.color = new Color(0f, 0f, 1f, 0.2f);
        Gizmos.DrawWireSphere(transform.position, rangedSettings.minimumRange);
    }
} 