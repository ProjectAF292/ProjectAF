using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public GameObject[] prefabs;
    [SerializeField]
    public List<GameObject>[] skills;

    public Transform attackPos;

    private void Awake()
    {
        skills = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < skills.Length; index++)
        {
            skills[index] = new List<GameObject> ();
        }

        Debug.Log(skills.Length);
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        foreach (GameObject item in skills[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (select == null)
        {
            select = Instantiate(prefabs[index], attackPos.transform);
            skills[index].Add(select);
        }

        return select;
    }

}
