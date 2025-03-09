using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 공격 지점을 마우스 위치에 따라 제어하는 클래스
/// 플레이어 주변에서 마우스 방향을 따라 회전하는 공격 포인트를 관리
/// </summary>
public class AttackPoint : MonoBehaviour
{
    [Header("Attack Point Settings")]
    [Tooltip("공격 포인트가 따라다닐 플레이어 오브젝트")]
    public Transform player;

    [Tooltip("플레이어로부터 공격 포인트까지의 거리")]
    public float distanceFromPlayer = 1.0f;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("AttackPoint: Main Camera를 찾을 수 없습니다!");
        }
    }

    private void LateUpdate()
    {
        if (player == null || mainCamera == null) return;

        // 마우스 위치 가져오기
        Vector3 mousePosition = Input.mousePosition;
        
        // 마우스 스크린 좌표를 월드 좌표로 변환
        Vector3 worldMousePosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.nearClipPlane));
        worldMousePosition.z = 0f;

        // 플레이어에서 마우스까지의 방향 계산
        Vector3 direction = (worldMousePosition - player.position).normalized;

        // 공격 포인트 위치 업데이트
        transform.position = player.position + (direction * distanceFromPlayer);

        // 공격 포인트 회전 업데이트
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Debug.DrawLine(player.position, transform.position, Color.red);
    }
}
