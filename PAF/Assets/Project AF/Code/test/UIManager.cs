using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject skillsui;
    public GameObject[] skills = new GameObject[6]; //여긴 나중에 무조건 바꿔야 함, List로 만들어서 활성화 된 스킬 ui 그대로 받아오게 해야 함
    public List<GameObject> skillList = new List<GameObject>(); //스킬 ui를 직접 연결이 아닌 자동 연결하게 하려면 이거 사용
    public GameObject[] skillSlot = new GameObject[3]; //여긴 나중에 개수 6개로 바꿔야 함 아니면 슬롯A, B로 나누던가
    public TextMeshProUGUI[] slotName = new TextMeshProUGUI[3]; //여기도 슬롯이랑 동일하게 가야 함

    public TextMeshProUGUI curSlotName; //일단 테스트용 ui, 지금 선택된 슬롯이 뭔지 알려주는 ui

    DataManager dataManager;
    UserData userData;

    int slotId;
    bool isSlotClicked;

    private void Start()
    {
        dataManager = DataManager.Instance;
        userData = dataManager.userData;
    }

    //스킬 변경 버튼 클릭
    public void SkillChange()
    {
        skillsui.SetActive(!skillsui.activeSelf); //스킬창 ui 활성화/비활성화 하기, 현재 상태의 반대로 바뀌는 방식
        //이 밑에 현재 유저가 보유중인 스킬의 종류와 동일한 개수의 스킬 ui를 활성화 시켜야 하기 때문에 그걸 받아와서 배열의 크기를 조정하고
        //활성화된 각 오브젝트에 스킬의 정보를 차례대로 넣어줘야 함
        //지금은 일단 스킬 3개밖에 없어서 3개만 해놨음

        if (skillsui.activeSelf)
        {
            SkillInfoChange(); //스킬 ui가 활성화되어 있으면 스킬창 ui 갱신하는 함수 호출
        }
        else
        {
            for (int i = 0; i < skills.Length; i++)
            {
                skills[i].SetActive(false);
            }
        }
    }

    //스킬 슬롯 클릭
    public void SlotClicked()
    {
        isSlotClicked = true; //슬롯이 클릭된 상태에서만 스킬이 장착되어야 함, 그래서 슬롯이 클릭 됐는지 판단하기 위한 변수가 필요
        GameObject selectSlot = EventSystem.current.currentSelectedGameObject; //지금 클릭한 오브젝트가 뭔지 받아오는 기능
        for (int i = 0; i < skillSlot.Length; i++) //지금 선택한게 슬롯이 맞는지 확인하려면 슬롯을 담고있는 배열을 한번 쭉 훑어야 함, 그래서 그 길이 만큼 반복
        {
            if (selectSlot == skillSlot[i].gameObject) //지금 선택한 오브젝트가 슬롯이 맞는지 확인
            {
                slotId = i;
            }
        }
    }

    //스킬 클릭
    public void SkillClicked()
    {
        if (isSlotClicked)
        {
            GameObject selectSkill = EventSystem.current.currentSelectedGameObject;
            SkillData skillData = selectSkill.GetComponent<SkillData>();

            var skillSlot = dataManager.isSlotASelected ? userData.atSlotA : userData.atSlotB;

            int curSlotChar = skillSlot[slotId];
            for (int i = 0; i < skillSlot.Length; i++)
            {
                if (skillData.skillId == skillSlot[i])
                {
                    skillSlot[i] = curSlotChar;
                    skillSlot[slotId] = skillData.skillId;
                }
                else
                {
                    skillSlot[slotId] = skillData.skillId;
                }
            }

            SkillInfoChange();
        }
    }

    //스킬 ui 갱신
    public void SkillInfoChange()
    {
        isSlotClicked = false;
        TextMeshProUGUI[] skillName = new TextMeshProUGUI[userData.atList.Count];

        var skillSlot = dataManager.isSlotASelected ? userData.atSlotA : userData.atSlotB;

        for (int i = 0; i < userData.atList.Count; i++)
        {
            skills[i].gameObject.SetActive(true);
            skills[i].gameObject.GetComponent<SkillData>().skillId = userData.atList[i];
        }

        for (int i = 0; i < skillName.Length; i++)
        {
            int row = dataManager.SearchForId(DataManager.Table.AtTable, userData.atList[i]);

            skillName[i] = skills[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            skillName[i].text = dataManager.atSkillTbl[row]["Desc"].ToString();
        }

        for (int i = 0; i < slotName.Length; i++)
        {
            int row = dataManager.SearchForId(DataManager.Table.AtTable, skillSlot[i]);

            slotName[i].text = dataManager.atSkillTbl[row]["Desc"].ToString();
        }
    }

    public void ChangeSlot()
    {
        var skillSlot = dataManager.isSlotASelected ? userData.atSlotA : userData.atSlotB;

        dataManager.isSlotASelected = !dataManager.isSlotASelected;

        curSlotName.text = dataManager.isSlotASelected ? "SlotA" : "SlotB";

        SkillInfoChange();
    }

    public void AddSkills()
    {
        for (int i = dataManager.atSkillTbl.Count; i > 2; i--)
        {
            userData.atList.Add((int)dataManager.atSkillTbl[i - 1]["Id"]);
        }
    }

    public void PauseUI()
    {
        Time.timeScale = 0;
    }
}
