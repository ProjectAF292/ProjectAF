using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;

public class TBaseEnemy : MonoBehaviour
{
    [SerializeField] public EnemyData enemyData;
    private float currentHealth;
    private bool isDead = false;
    protected Animator animator;
    protected IEnemyAttack attackComponent;
    protected IEnemyAttribute attributeComponent;
    protected List<IEnemySkill> skillComponents = new List<IEnemySkill>();
    private Rigidbody2D rb;

    [Header("UI Components")]
    [SerializeField] private Canvas healthBarCanvas;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private float healthBarYOffset = 0.2f; // 체력바 Y축 오프셋
    [SerializeField] private Vector2 healthBarSize = new Vector2(0.35f, 0.03f); // 체력바 크기

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (healthBarCanvas != null)
        {
            RectTransform canvasRect = healthBarCanvas.GetComponent<RectTransform>();
            if (canvasRect != null)
            {
                canvasRect.sizeDelta = healthBarSize;
                canvasRect.localPosition = new Vector3(0, healthBarYOffset, 0);
            }
        }
    }
#endif

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (animator == null)
        {
            animator = gameObject.AddComponent<Animator>();
        }
        if (enemyData != null)
        {
            animator.runtimeAnimatorController = enemyData.animatorController;
            currentHealth = enemyData.maxHealth;
            Debug.Log($"Enemy HP initialized: {currentHealth}/{enemyData.maxHealth}");
        }
        else
        {
            Debug.LogError("EnemyData is not assigned!");
        }
        isDead = false;
    }

    private void Start()
    {
        // 체력바 UI 초기화
        InitializeHealthBar();
    }

    private void InitializeHealthBar()
    {
        if (healthBarCanvas == null)
        {
            // 캔버스 생성
            GameObject canvasObj = new GameObject("HealthBarCanvas");
            healthBarCanvas = canvasObj.AddComponent<Canvas>();
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            
            // 캔버스 설정
            healthBarCanvas.renderMode = RenderMode.WorldSpace;
            healthBarCanvas.worldCamera = Camera.main;
            canvasObj.transform.SetParent(transform);
            healthBarCanvas.sortingOrder = 5;
            
            // 위치 설정
            RectTransform canvasRect = canvasObj.GetComponent<RectTransform>();
            canvasRect.sizeDelta = healthBarSize;
            canvasRect.localPosition = new Vector3(0, healthBarYOffset, 0);
            canvasRect.localScale = new Vector3(1, 1, 1);

            // 배경 패널 생성
            GameObject bgObj = new GameObject("HealthBarBG");
            Image bgImage = bgObj.AddComponent<Image>();
            bgImage.color = Color.black;
            bgObj.transform.SetParent(canvasRect, false);
            RectTransform bgRect = bgObj.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.sizeDelta = Vector2.zero;

            // 체력바 Fill 생성
            GameObject fillObj = new GameObject("HealthBarFill");
            healthBarFill = fillObj.AddComponent<Image>();
            healthBarFill.color = Color.red;
            fillObj.transform.SetParent(bgObj.transform, false);
            RectTransform fillRect = fillObj.GetComponent<RectTransform>();
            fillRect.anchorMin = new Vector2(0, 0);
            fillRect.anchorMax = new Vector2(1, 1);
            fillRect.sizeDelta = Vector2.zero;
            fillRect.pivot = new Vector2(1, 0.5f); // 피벗을 오른쪽 중앙으로 설정
        }

        UpdateHealthBar();
    }

    private void LateUpdate()
    {
        if (healthBarCanvas != null)
        {
            // 체력바가 항상 카메라를 향하도록 설정
            healthBarCanvas.transform.rotation = Camera.main.transform.rotation;
        }
    }

    public virtual void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        Debug.Log($"Enemy took damage: {damage}, Current HP: {currentHealth}/{enemyData.maxHealth}");
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        
        isDead = true;
        currentHealth = 0;
        UpdateHealthBar();

        // 사망 애니메이션 재생
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // 체력바 비활성화
        if (healthBarCanvas != null)
        {
            healthBarCanvas.gameObject.SetActive(false);
        }

        // 컴포넌트들 비활성화
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false;
        }

        // 사망 애니메이션 재생 후 오브젝트 파괴
        Destroy(gameObject, 0.5f);
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill != null && enemyData != null)
        {
            float healthPercent = Mathf.Clamp01(currentHealth / enemyData.maxHealth);
            // Fill의 scale을 조절하여 체력바 업데이트
            healthBarFill.transform.localScale = new Vector3(healthPercent, 1, 1);
            Debug.Log($"Health bar updated: {healthPercent:P0}");
        }
    }

    public void SetEnemyData(EnemyData data)
    {
        enemyData = data;
    }

    public void AddAttackComponent(IEnemyAttack attack)
    {
        attackComponent = attack;
        attack.Initialize(enemyData);
    }

    public void AddAttributeComponent(IEnemyAttribute attribute)
    {
        attributeComponent = attribute;
        attribute.Initialize(enemyData);
    }

    public void AddSkillComponent(IEnemySkill skill)
    {
        skillComponents.Add(skill);
        skill.Initialize(enemyData);
    }

    public bool IsDead() => isDead;
    public float GetCurrentHealth() => currentHealth;
    public float GetMaxHealth() => enemyData.maxHealth;
} 