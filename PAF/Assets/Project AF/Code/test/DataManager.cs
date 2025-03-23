using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    public int[] skillSlotA = new int[3];
    public int[] skillSlotB = new int[3];
    public List<int> skillList = new List<int>();
}

public class DataManager : Singleton<DataManager>
{
    public UserData userData = new UserData();
    public int curSlotNum;
    public bool isSlotASelected;

    public List<Dictionary<string , object>> skillTbl;

    void Awake()
    {
        skillTbl = CSVReader.Read("DT_Skill");
        curSlotNum = 0;
        isSlotASelected = true;

        for (int i = 0; i < 3; i++)
        {
            userData.skillSlotA[i] = i + 1;
            userData.skillSlotB[i] = i + 1;
        }

        for (int i = 0; i < 3; i++)
        {
            userData.skillList.Add(i + 1);
        }

        int row = 0;

        //값으로 행 조회 테스트
        for (int i = 0; i < skillTbl.Count; i++)
        {
            if (skillTbl[i]["Desc"].ToString() == "Attack 1" )
            {
                row = i;
            }
        }

        Debug.Log(skillTbl[row]["Id"]);
    }
}
