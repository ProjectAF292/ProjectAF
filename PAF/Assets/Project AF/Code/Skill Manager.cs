using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public GameObject[] attacks; // 스킬(마도구)들을 보관할 변수

    List<GameObject>[] skills; // 스킬(마도구 이하 생략)의 리스트

    private void Awake()
    {
        skills = new List<GameObject>[attacks.Length]; // 스킬의 수와 리스트의 수가 동일하게 만들어 주는 함수

        for (int index = 0; index < skills.Length; index++)
        {
            skills[index] = new List<GameObject>();
        }

    }

    public GameObject Get(int index) // 스킬 시전하면 해당 스킬을 찾아서 넣으라는 코드
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

        if (!select)
        {
            select = Instantiate(attacks[index], transform);
            skills[index].Add(select);
        }

        return select;
    }

}
