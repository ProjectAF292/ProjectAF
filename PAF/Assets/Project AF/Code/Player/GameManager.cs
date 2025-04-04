using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isPaused;

    public GameObject puaseUI;

    private void Awake()
    {
        isPaused = false;
    }

    public void Pause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0.0f;
            puaseUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1.0f;
            puaseUI.SetActive(false);
        }
    }
}