using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
        
    public Vector2 inputVec;
    public float speed;
        
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

        void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
       
    }

    private void Update()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime; // 플레이어 이동 입렵값을 일정하게 유지
        rigid.MovePosition(rigid.position + nextVec); //리지드 위치에 내가 입력한 좌표값 더한곳으로 이동
    }

                  
    private void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.magnitude); // 이동에 따라서 캐릭터가 바라보고 있는 방향 지정

        if (inputVec.x != 0) 
        {
            spriter.flipX = inputVec.x < 0; // 지금 백터x의 값이 0보다 작으면 스프라이트를 x 값으로 플립 해라
        }
    }
   

   
       
             
         
}
