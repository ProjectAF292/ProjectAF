using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 플레이어의 체력바 UI를 관리하는 클래스
/// </summary>
public class PlayerHealthBar : MonoBehaviour
{
    [Header("References")]
    [Tooltip("체력바를 표시할 플레이어 오브젝트")]
    public PlayerHealth player;
    
    [Tooltip("체력바 Fill 이미지")]
    public Image healthBarFill;
    
    [Tooltip("체력바의 위치 오프셋")]
    public Vector3 offset = new Vector3(0, 0.5f, 0);

    private Camera _mainCamera;
    private RectTransform _fillRectTransform;

    private void Awake()
    {
        // 컴포넌트 캐싱
        _mainCamera = Camera.main;
        if (healthBarFill != null)
        {
            _fillRectTransform = healthBarFill.rectTransform;
        }
    }

    private void Start()
    {
        // 필수 컴포넌트 체크
        ValidateComponents();
    }

    /// <summary>
    /// 필수 컴포넌트가 할당되었는지 확인
    /// </summary>
    private void ValidateComponents()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is missing!");
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
        // 플레이어가 죽었거나 없으면 체력바 제거
        if (player == null || player.IsDead())
        {
            Destroy(gameObject);
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
        // 플레이어의 위치에 오프셋을 더한 위치로 이동
        transform.position = player.transform.position + offset;
        
        // 항상 카메라를 향하도록 회전
        transform.rotation = _mainCamera.transform.rotation;
    }

    /// <summary>
    /// 체력바 Fill 업데이트
    /// </summary>
    private void UpdateHealthBarFill()
    {
        // 현재 체력 비율 계산 (0~1 사이의 값으로 제한)
        float healthPercent = Mathf.Clamp01(player.GetCurrentHealth() / player.GetMaxHealth());
        
        // Fill 이미지의 크기 조절
        _fillRectTransform.localScale = new Vector3(healthPercent, 1, 1);
    }
} 