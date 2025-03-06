using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public GameObject[] attacks; // ��ų(������)���� ������ ����

    List<GameObject>[] skills; // ��ų(������ ���� ����)�� ����Ʈ

    private void Awake()
    {
        skills = new List<GameObject>[attacks.Length]; // ��ų�� ���� ����Ʈ�� ���� �����ϰ� ����� �ִ� �Լ�

        for (int index = 0; index < skills.Length; index++)
        {
            skills[index] = new List<GameObject>();
        }

    }

    public GameObject Get(int index) // ��ų �����ϸ� �ش� ��ų�� ã�Ƽ� ������� �ڵ�
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
