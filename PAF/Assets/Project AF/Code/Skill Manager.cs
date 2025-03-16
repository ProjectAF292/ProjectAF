using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public GameObject[] prefabs;

    //public List<GameObject>[] skills;
    public List<GameObject> skills = new List<GameObject>();

    public Transform attackPos;

    public GameObject skillPool;

    int skillCount;

    private void Awake()
    {
        //skills = new List<GameObject>[prefabs.Length];
        skillCount = 0;

        //for (int index = 0; index < skills.Length; index++)
        //{
        //    skills[index] = new List<GameObject> ();
        //}

        //Debug.Log(skills.Length);
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        //foreach (GameObject item in skills[skillCount])
        //{
        //    if (!item.activeSelf)
        //    {
        //        select = item;
        //        select.SetActive(true);
        //        break;
        //    }
        //}

        if (select == null)
        {
            select = Instantiate(prefabs[index], attackPos.transform.position, Quaternion.identity, skillPool.gameObject.transform);
            //skills[index].Add(select);
            skills.Add(select);
            skillCount++;

        }
        Debug.Log(index);

        return select;
    }

}
