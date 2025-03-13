using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{

    public int testInt = 1;

    public List<Dictionary<string, object>> testTbl;

    // Start is called before the first frame update
    void Start()
    {
        testTbl = CSVReader.Read("TestTable");

        for (int i = 0; i < testTbl.Count; i++)
        {
            Debug.Log(testTbl[i]["Id"].ToString() + ", " + testTbl[i]["Name"].ToString() + ", " + testTbl[i]["Desc"].ToString() + testTbl[i]["Introduce"].ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
