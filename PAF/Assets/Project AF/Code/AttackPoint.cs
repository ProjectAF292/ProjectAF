using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 플레이어의 공격 지점을 마우스 위치에 따라 제어하는 클래스
/// 플레이어 주변에서 마우스 방향을 따라 회전하는 공격 포인트를 관리
/// </summary>
public class AttackPoint : MonoBehaviour
{
    public Vector2 mousePos;


    [Header("조준점")]
    public Transform attackPoint; // 조준점으로 사용할 오브젝트
    public float radius = 2f; // 회전 반경



}
        
