using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class UserData
{
    public int[] atSlotA = new int[2];
    public int[] atSlotB = new int[2];
    public int[] weaponSkill = new int[2];
    public List<int> atList = new List<int>();
    public List<int> weaponList = new List<int>();
}

public class DataManager : Singleton<DataManager>
{
    public UserData userData = new UserData();
    public int curSlotNum;
    public bool isSlotASelected;

    public List<Dictionary<string , object>> atSkillTbl;
    public List<Dictionary<string , object>> weaponSkillTbl;

    public enum Table
    {
        AtTable,
        WeaponTable
    }

    void Awake()
    {
        atSkillTbl = CSVReader.Read("DT_Skill_Artifact");
        weaponSkillTbl = CSVReader.Read("DT_Skill_Weapon");
        curSlotNum = 0;
        isSlotASelected = true;
        
        for (int i = 0; i < 2; i++)
        {
            userData.atList.Add((int)atSkillTbl[i]["Id"]);
            userData.atSlotA[i] = (int)atSkillTbl[i]["Id"];
            userData.atSlotB[i] = (int)atSkillTbl[i]["Id"];
            userData.weaponList.Add((int)weaponSkillTbl[i]["Id"]);
            userData.weaponSkill[i] = (int)weaponSkillTbl[i]["Id"];
        }

    }

    public int SearchForId(Table table, int id)
    {
        int row = 0;

        List<Dictionary<string, object>> tbl = null;

        switch (table)
        {
            case Table.AtTable:
                tbl = atSkillTbl;
                break;
            case Table.WeaponTable:
                tbl = weaponSkillTbl;
                break;
        }

        for (int i = 0; i < tbl.Count; i++)
        {
            if (tbl[i]["Id"].ToString() == id.ToString())
            {
                row = i;
            }
        }

        return row;
    }
}
