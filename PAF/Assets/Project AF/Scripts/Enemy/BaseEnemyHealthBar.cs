using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 모든 적 체력바의 기본 클래스
/// </summary>
public abstract class BaseEnemyHealthBar : MonoBehaviour
{
    [Header("References")]
    [Tooltip("체력바 Fill 이미지")]
    public Image healthBarFill;

    [Tooltip("체력바의 위치 오프셋")]
    public Vector3 offset = new Vector3(0, 0.7f, 0);

    private Camera _mainCamera;
    private RectTransform _fillRectTransform;
    protected BaseEnemy _enemy;

    protected virtual void Awake()
    {
        // 컴포넌트 캐싱
        _enemy = GetComponentInParent<BaseEnemy>();
        _mainCamera = Camera.main;
        if (healthBarFill != null)
        {
            _fillRectTransform = healthBarFill.rectTransform;
        }

        ValidateComponents();
    }

    /// <summary>
    /// 필수 컴포넌트가 할당되었는지 확인
    /// </summary>
    private void ValidateComponents()
    {
        if (_enemy == null)
        {
            Debug.LogError("Enemy reference is missing!");
            enabled = false;
            return;
        }

        if (healthBarFill == null)
        {
            Debug.LogError("HealthBarFill reference is missing!");
            enabled = false;
            return;
        }

        if (_mainCamera == null)
        {
            Debug.LogError("Main Camera not found!");
            enabled = false;
            return;
        }
    }

    private void LateUpdate()
    {
        if (_enemy == null || _enemy.IsDead())
        {
            gameObject.SetActive(false);
            return;
        }

        UpdateHealthBarPosition();
        UpdateHealthBarFill();
    }

    /// <summary>
    /// 체력바 위치 업데이트
    /// </summary>
    private void UpdateHealthBarPosition()
    {
        // 적의 위치에 오프셋을 더한 위치로 이동
        transform.position = _enemy.transform.position + offset;
        
        // 항상 카메라를 향하도록 회전
        transform.rotation = _mainCamera.transform.rotation;
    }

    /// <summary>
    /// 체력바 Fill 업데이트
    /// </summary>
    private void UpdateHealthBarFill()
    {
        // 현재 체력 비율 계산 (0~1 사이의 값으로 제한)
        float healthPercent = _enemy.GetHealthRatio();
        
        // Fill 이미지의 크기 조절
        _fillRectTransform.localScale = new Vector3(healthPercent, 1, 1);
    }
} 