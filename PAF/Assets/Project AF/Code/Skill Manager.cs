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

        var skillSlot = dataManager.isSlotASelected ? userData.skillSlotA : userData.skillSlotB;

        if (select == null)
        {
            select = Instantiate(Resources.Load<GameObject>(dataManager.skillTbl[skillSlot[index]]["Prefab"].ToString()), attackPos.transform.position, Quaternion.identity, skillPool.gameObject.transform);
        }
    }

    public void NormalAttack()
    {
        GameObject select = null;

        if (select == null)
        {
            select = Instantiate(Resources.Load<GameObject>(dataManager.skillTbl[0]["Prefab"].ToString()), attackPos.transform.position, Quaternion.identity, skillPool.gameObject.transform);
        }
    }
}