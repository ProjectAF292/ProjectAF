using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Player : MonoBehaviour
{
    [Header("Move Set")]
    public Vector2 inputVec;
    public float speed;
    [Header("Can't Move")]
    public LayerMask layer;
    
    CapsuleCollider2D coll2d;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

       
        void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll2d = GetComponent<CapsuleCollider2D>();

    }

    private void Update()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
                        
    }

    private void FixedUpdate()
    {
        
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime; // 플레이어 이동 입렵값을 일정하게 유지

        //RaycastHit2D hit; // 이동을 못하게 하는 로직
        //Vector2 start = transform.position;
        //Vector2 end = start + new Vector2(nextVec.x, nextVec.y);

        //coll2d.enabled = false;
        //hit = Physics2D.Linecast(start, end, layer);
        //coll2d.enabled = true;

        //if (hit.transform == null)
        {
            //rigid.MovePosition(rigid.position + nextVec); //리지드 위치에 내가 입력한 좌표값 더한곳으로 이동
        }
        rigid.MovePosition(rigid.position + nextVec); //리지드 위치에 내가 입력한 좌표값 더한곳으로 이동
        rigid.velocity = new Vector2(inputVec.x, inputVec.y);
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
