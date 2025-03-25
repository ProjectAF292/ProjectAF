using UnityEngine;

/// <summary>
/// 탱커 적의 AI를 처리하는 클래스
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(TankEnemy))]
[AddComponentMenu("")]  // Inspector에서 숨김
public class TankEnemyAI : BaseEnemyAI
{
    private TankEnemyAttack _attackComponent;
    private bool _isPlayerInRange;

    [System.Serializable]
    public class TankSettings
    {
        [Header("Movement Settings")]
        [Tooltip("탱커 적이 플레이어를 감지하는 범위")]
        public float detectionRange = 8f;

        [Tooltip("탱커 적이 플레이어를 공격할 수 있는 범위")]
        public float attackRange = 1.5f;

        [Tooltip("탱커 적의 이동 속도")]
        public float moveSpeed = 1.5f;
    }

    [Header("Tank Enemy Settings")]
    [SerializeField]
    private TankSettings tankSettings = new TankSettings();

    protected override void Awake()
    {
        base.Awake();
        _attackComponent = GetComponent<TankEnemyAttack>();
        
        if (_attackComponent == null)
        {
            Debug.LogError("TankEnemyAttack component not found!");
        }
    }

    protected override void HandleAIBehavior()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);
        Vector2 directionToPlayer = ((Vector2)_player.position - (Vector2)transform.position).normalized;
        
        _isPlayerInRange = distanceToPlayer <= tankSettings.detectionRange;
        
        if (_isPlayerInRange)
        {
            // 공격 범위 내에 있으면 공격
            if (distanceToPlayer <= tankSettings.attackRange)
            {
                _rb.velocity = Vector2.zero;
                UpdateDirection(directionToPlayer);
                _attackComponent.TryAttack();
            }
            // 공격 범위 밖에 있으면 플레이어를 추격
            else
            {
                _rb.velocity = directionToPlayer * tankSettings.moveSpeed;
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
        }
    }

    private void UpdateAnimation(Vector2 direction)
    {
        if (_animator == null) return;
        
        bool isMoving = _rb.velocity.magnitude > 0.01f;
        _animator.SetBool("isMoving", isMoving);
        
        if (isMoving)
        {
            UpdateDirection(direction);
        }
    }

    private void OnDrawGizmos()
    {
        // 감지 범위 시각화 (빨간색)
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, tankSettings.detectionRange);

        // 공격 범위 시각화 (파란색)
        Gizmos.color = new Color(0f, 0f, 1f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, tankSettings.attackRange);
    }
} 