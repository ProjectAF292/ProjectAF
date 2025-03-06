using UnityEngine;

/// <summary>
/// 적의 기본 속성과 상태를 관리하는 클래스
/// </summary>
public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [Tooltip("적의 이름")]
    public string enemyName = "Enemy";
    
    [Tooltip("최대 체력")]
    public float maxHealth = 100f;
    
    [Tooltip("현재 체력")]
    [SerializeField]
    private float _currentHealth;
    public float CurrentHealth => _currentHealth; // 읽기 전용 프로퍼티

    // 사망 상태를 외부에서 읽을 수만 있도록 프로퍼티로 설정
    public bool IsDead { get; private set; }

    // 캐싱
    private Rigidbody2D _rb;

    private void Awake()
    {
        // 컴포넌트 캐싱
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // 초기 상태 설정
        InitializeEnemy();
    }

    /// <summary>
    /// 적의 초기 상태를 설정
    /// </summary>
    private void InitializeEnemy()
    {
        _currentHealth = maxHealth;
        IsDead = false;
    }

    /// <summary>
    /// 적이 데미지를 받았을 때 호출되는 메서드
    /// </summary>
    /// <param name="damage">받은 데미지량</param>
    public void TakeDamage(float damage)
    {
        // 이미 사망한 상태면 데미지를 받지 않음
        if (IsDead) return;
        
        _currentHealth -= damage;
        
        // 체력이 0 이하가 되면 사망 처리
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }

        // 여기에 데미지를 받았을 때의 효과나 애니메이션을 추가할 수 있습니다.
    }

    /// <summary>
    /// 적이 사망했을 때 호출되는 메서드
    /// </summary>
    private void Die()
    {
        IsDead = true;
        
        // Rigidbody가 있다면 움직임을 멈춤
        if (_rb != null)
        {
            _rb.velocity = Vector2.zero;
        }

        // 여기에 사망 애니메이션이나 파티클 효과를 추가할 수 있습니다.
        
        // 1초 후에 오브젝트 제거
        Destroy(gameObject, 1f);
    }
} 