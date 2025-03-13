using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void Onclick()
    {
        DataManager.Instance.testInt++;

        SceneManager.LoadScene("JSH Scenes");
    }
}
