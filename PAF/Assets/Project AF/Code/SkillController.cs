using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    public SkillManager ski;
    private Camera _camera;

    public Transform player;
    public float radius = 2f;

    private void Start()
    {
        _camera = Camera.main;
    }


    void Update()
    {
        
        if (player == null) return;

        Vector2 mousePos = (Vector2)_camera.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mousePos.y - player.position.y, mousePos.x - player.position.x);

        float x = player.position.x + Mathf.Cos(angle) * radius;
        float y = player.position.y + Mathf.Sin(angle) * radius;
        transform.position = new Vector2(x, y);

        Vector2 dirVec = (mousePos - (Vector2)transform.position).normalized;
        transform.up = dirVec;

        if (Input.GetMouseButtonDown(0)) ski.Get(0);
        if (Input.GetMouseButtonDown(1)) ski.Get(1);
        if (Input.GetKeyDown(KeyCode.Q)) ski.Get(2);
        if (Input.GetKeyDown(KeyCode.E)) ski.Get(3);

    }

    
}
