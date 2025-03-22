using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    public int[] skillSlot = new int[3]; //SkillManager�� ���ֵ��� ���߿� ���� �� �ٲ� �� ���� �ٲ���� ��
}

public class DataManager : Singleton<DataManager>
{
    public UserData userData = new UserData();

    public List<Dictionary<string , object>> skillTbl;

    void Awake()
    {
        skillTbl = CSVReader.Read("DT_Skill");

        for (int i = 0; i < userData.skillSlot.Length; i++)
        {
            userData.skillSlot[i] = i + 1;
        }
    }
}
