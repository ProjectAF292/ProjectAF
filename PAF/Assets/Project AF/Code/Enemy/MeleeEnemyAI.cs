using UnityEngine;

/// <summary>
/// 근접 공격 적의 AI를 처리하는 클래스
/// </summary>
public class MeleeEnemyAI : BaseEnemyAI
{
    private MeleeEnemyAttack _attackComponent;
    private bool _isPlayerInRange;

    [System.Serializable]
    public class MeleeSettings
    {
        [Tooltip("근접 적이 플레이어를 감지하는 범위")]
        public float detectionRange = 5f;

        [Tooltip("근접 적이 플레이어를 공격하는 범위")]
        public float attackRange = 2f;

        [Tooltip("근접 적의 이동 속도")]
        public float moveSpeed = 2f;
    }

    [Header("Melee Enemy Settings")]
    [SerializeField]
    private MeleeSettings meleeSettings = new MeleeSettings();

    protected override void Awake()
    {
        base.Awake();
        _attackComponent = GetComponent<MeleeEnemyAttack>();
        
        if (_attackComponent == null)
        {
            Debug.LogError("MeleeEnemyAttack component not found!");
        }
    }

    protected override void Start()
    {
        base.Start();
        _isPlayerInRange = false;
    }

    protected override void FixedUpdate()
    {
        if (_enemy != null && _enemy.IsDead()) return;
        if (_player == null)
        {
            FindPlayer();
            HandleIdle();
            return;
        }

        HandleAIBehavior();
    }

    protected override void HandleAIBehavior()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);
        Vector2 directionToPlayer = ((Vector2)_player.position - (Vector2)transform.position).normalized;
        
        _isPlayerInRange = distanceToPlayer <= meleeSettings.detectionRange;
        
        if (_isPlayerInRange)
        {
            // 공격 범위 내에 있으면 공격
            if (distanceToPlayer <= meleeSettings.attackRange)
            {
                _rb.velocity = Vector2.zero;
                UpdateDirection(directionToPlayer);
                _attackComponent.TryAttack();
            }
            // 공격 범위 밖에 있으면 플레이어를 추격
            else
            {
                _rb.velocity = directionToPlayer * meleeSettings.moveSpeed;
                UpdateAnimation(directionToPlayer);
            }
        }
        else
        {
            HandleIdle();
        }
    }

    private void HandleIdle()
    {
        _rb.velocity = Vector2.zero;
        _lastDirection = Vector2.down;
        
        if (_animator != null)
        {
            _animator.SetBool("isMoving", false);
            _animator.SetFloat("moveX", 0);
            _animator.SetFloat("moveY", -1);
            if (_spriteRenderer != null)
            {
                _spriteRenderer.flipX = false;
            }
        }
    }

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
        else // 아래
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
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, meleeSettings.detectionRange);

        // 공격 범위 시각화 (파란색)
        Gizmos.color = new Color(0f, 0f, 1f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, meleeSettings.attackRange);
    }
} 