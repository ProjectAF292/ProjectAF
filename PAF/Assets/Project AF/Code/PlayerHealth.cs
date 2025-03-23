using System.Collections;
using UnityEngine;

/// <summary>
/// 플레이어의 체력을 관리하는 클래스
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [Tooltip("최대 체력")]
    public float maxHealth = 100f;

    [Tooltip("현재 체력")]
    [SerializeField]
    private float currentHealth;

    // 플레이어의 사망 상태
    private bool isDead = false;

    // 컴포넌트 캐싱
    private Animator animator;
    private Rigidbody2D rb;
    WaitForFixedUpdate wait;

    private void Awake()
    {
        // 컴포넌트 캐싱
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        wait = new WaitForFixedUpdate();
    }

    private void Start()
    {
        // 초기 체력 설정
        currentHealth = maxHealth;
    }

   
    public void TakeDamage(float damage)
    {
       
        if (isDead) return;

        currentHealth -= damage;
        //Debug.Log($"플레이어가 {damage} 데미지를 받았습니다. 현재 체력: {currentHealth}");

        // 체력이 0 이하가 되면 사망
        if (currentHealth <= 0)
        {
            Dead();
        }

        else
        {
            animator.SetTrigger("Hit");
        }

        
    }

    /// <summary>
    /// 플레이어 사망 처리
    /// </summary>
    private void Dead()
    {
        if (isDead) return;

        isDead = true;
        currentHealth = 0;

        Debug.Log("플레이어가 사망했습니다!");

        animator.SetTrigger("Dead");
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }
    }

    /// <summary>
    /// 현재 체력을 반환
    /// </summary>
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    /// <summary>
    /// 최대 체력을 반환
    /// </summary>
    public float GetMaxHealth()
    {
        return maxHealth;
    }

    /// <summary>
    /// 사망 상태를 반환
    /// </summary>
    public bool IsDead()
    {
        return isDead;
    }
} 