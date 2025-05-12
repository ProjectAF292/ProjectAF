using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public Transform attackPos;

    public GameObject skillPool;
    
    DataManager dataManager;
    UserData userData;

    private void Start()
    {
        dataManager = DataManager.Instance;
        userData = dataManager.userData;
    }

    public void UseSkill(int index)
    {
        GameObject select = null;

        var skillSlot = dataManager.isSlotASelected ? userData.atSlotA : userData.atSlotB;

        int skillIndex = dataManager.SearchForId(DataManager.Table.AtTable, skillSlot[index]);
        string prefabName = dataManager.atSkillTbl[skillIndex]["Prefab"].ToString();
        GameObject prefab = Resources.Load<GameObject>(prefabName);

        if (select == null)
        {
            select = Instantiate(prefab, attackPos.transform.position, Quaternion.identity, skillPool.gameObject.transform);
        }
    }

    public void NormalAttack(int index)
    {
        GameObject select = null;

        int skillIndex = dataManager.SearchForId(DataManager.Table.WeaponTable, userData.weaponSkill[index]);
        string prefabName = dataManager.weaponSkillTbl[skillIndex]["Prefab"].ToString();
        GameObject prefab = Resources.Load<GameObject>(prefabName);

        if (select == null)
        {
            select = Instantiate(prefab, attackPos.transform.position, Quaternion.identity, skillPool.gameObject.transform);
        }
    }
}