using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    public Transform player; // 플레이어 오브젝트
    public float distanceFromPlayer = 1.0f; // 플레이어와 삼각형 사이 거리

    void Update()
    {
        if (player == null) return;

        // 마우스 위치를 월드 좌표로 변환
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // 2D 환경에서는 Z를 0으로 설정

        // 플레이어 위치 기준 방향 벡터
        Vector3 direction = (mousePosition - player.position).normalized;

        // 삼각형 위치를 플레이어에서 일정 거리 떨어진 곳으로 설정
        transform.position = player.position + direction * distanceFromPlayer;

        // 회전 각도 계산 (라디안을 각도로 변환)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
