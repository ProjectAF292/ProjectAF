using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    public Camera _camera;
    public GameObject _gameObject;

    void Update()
    {
        Vector2 mousePos = (Vector2)_camera.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = mousePos;

        if (Input.GetMouseButton(0)) _gameObject.SetActive(true);
    }
}
