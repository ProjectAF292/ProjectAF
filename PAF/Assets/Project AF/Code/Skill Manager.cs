using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public GameObject[] prefabs;

    public List<GameObject> skills = new List<GameObject>();

    public Transform attackPos;

    public GameObject skillPool;
    
    DataManager dataManager;
    UserData userData;

    private void Start()
    {
        dataManager = DataManager.Instance;
        userData = dataManager.userData;
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        if (select == null)
        {
            select = Instantiate(Resources.Load<GameObject>(dataManager.skillTbl[index]["Prefab"].ToString()), attackPos.transform.position, Quaternion.identity, skillPool.gameObject.transform);
        }
        return select;
    }
}