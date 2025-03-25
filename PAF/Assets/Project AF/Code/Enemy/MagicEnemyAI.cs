using UnityEngine;

/// <summary>
/// 마법 공격 적의 AI를 처리하는 클래스
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(MagicEnemy))]
[AddComponentMenu("")]  // Inspector에서 숨김
public class MagicEnemyAI : BaseEnemyAI
{
    private MagicEnemyAttack _attackComponent;
    private bool _isPlayerInRange;

    [System.Serializable]
    public class MagicSettings
    {
        [Header("Movement Settings")]
        [Tooltip("마법 적이 플레이어를 감지하는 범위")]
        public float detectionRange = 10f;

        [Tooltip("마법 적이 플레이어를 공격할 수 있는 범위")]
        public float attackRange = 7f;

        [Tooltip("마법 적의 이동 속도")]
        public float moveSpeed = 2f;

        [Tooltip("마법 적이 플레이어와 유지하려는 최소 거리")]
        public float minimumRange = 5f;

        [Header("Teleport Settings")]
        [Tooltip("텔레포트 시 플레이어 기준 랜덤 반경")]
        public float teleportRadius = 4f;

        [Tooltip("텔레포트 쿨타임 (초)")]
        public float teleportCooldown = 5f;
    }

    [Header("Magic Enemy Settings")]
    [SerializeField]
    private MagicSettings magicSettings = new MagicSettings();

    private float nextTeleportTime = 0f;
    private bool isTeleporting = false;
    private bool hasDetectedPlayer = false;

    protected override void Awake()
    {
        base.Awake();
        _attackComponent = GetComponent<MagicEnemyAttack>();
        
        if (_attackComponent == null)
        {
            Debug.LogError("MagicEnemyAttack component not found!");
        }
    }

    protected override void HandleAIBehavior()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);
        Vector2 directionToPlayer = ((Vector2)_player.position - (Vector2)transform.position).normalized;
        
        _isPlayerInRange = distanceToPlayer <= magicSettings.detectionRange;
        
        // 플레이어가 감지 범위 안에 있을 때
        if (_isPlayerInRange)
        {
            // 최초 감지 또는 쿨타임이 끝났을 때 텔레포트
            if ((!hasDetectedPlayer || Time.time >= nextTeleportTime))
            {
                hasDetectedPlayer = true;
                Teleport();
                return;
            }

            // 일반 행동 처리
            // 공격 범위 내에 있고 최소 거리보다 멀리 있으면 공격
            if (distanceToPlayer <= magicSettings.attackRange && distanceToPlayer >= magicSettings.minimumRange)
            {
                _rb.velocity = Vector2.zero;
                UpdateDirection(directionToPlayer);
                _attackComponent.TryAttack();
            }
            // 최소 거리보다 가까이 있으면 도망
            else if (distanceToPlayer < magicSettings.minimumRange)
            {
                _rb.velocity = -directionToPlayer * magicSettings.moveSpeed;
                UpdateAnimation(-directionToPlayer);
            }
            // 공격 범위 밖에 있으면 플레이어를 추격
            else
            {
                _rb.velocity = directionToPlayer * magicSettings.moveSpeed;
                UpdateAnimation(directionToPlayer);
            }
        }
        else
        {
            hasDetectedPlayer = false;
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

    private void Teleport()
    {
        if (isTeleporting) return;

        isTeleporting = true;
        nextTeleportTime = Time.time + magicSettings.teleportCooldown;

        // 플레이어 주변 랜덤 위치 계산
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float randomDistance = magicSettings.teleportRadius;
        Vector2 offset = new Vector2(
            Mathf.Cos(randomAngle) * randomDistance,
            Mathf.Sin(randomAngle) * randomDistance
        );
        Vector2 teleportPosition = (Vector2)_player.position + offset;

        // 텔레포트 실행
        transform.position = teleportPosition;
        
        // 애니메이션 처리
        if (_animator != null)
        {
            _animator.SetBool("isMoving", false);
            Vector2 directionToPlayer = ((Vector2)_player.position - teleportPosition).normalized;
            UpdateDirection(directionToPlayer);
        }

        isTeleporting = false;
    }

    private void OnDrawGizmos()
    {
        // 감지 범위 시각화 (빨간색)
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, magicSettings.detectionRange);

        // 공격 범위 시각화 (파란색)
        Gizmos.color = new Color(0f, 0f, 1f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, magicSettings.attackRange);

        // 최소 거리 시각화 (노란색)
        Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, magicSettings.minimumRange);
    }
} 