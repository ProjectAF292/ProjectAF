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
    public GameObject[] skills = new GameObject[3]; //여긴 나중에 무조건 바꿔야 함, List로 만들어서 활성화 된 스킬 ui 그대로 받아오게 해야 함
    public GameObject[] skillSlot = new GameObject[3]; //여긴 나중에 개수 6개로 바꿔야 함 아니면 슬롯A, B로 나누던가
    public TextMeshProUGUI[] slotName = new TextMeshProUGUI[3]; //여기도 슬롯이랑 동일하게 가야 함
    
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
                slotId = i; //슬롯 개수만큼 반복 돌려서 내가 지금 누른게 슬롯이 맞으면 몇번째 슬롯인지 받아오기
            }
        }
    }

    //스킬 클릭
    public void SkillClicked()
    {
        if (isSlotClicked)
        {
            GameObject selectSkill = EventSystem.current.currentSelectedGameObject; //마찬가지로 지금 클릭한 오브젝트가 뭔지 받아오기
            SkillData skillData = selectSkill.GetComponent<SkillData>(); //그리고 그 오브젝트가 가지고 있는 스킬 정보를 참조

            for (int i = 0; i < skills.Length; i++) //슬롯 부분이랑 동일
            {
                if (selectSkill == skills[i].gameObject) //슬롯 부분이랑 동일
                {
                    skillData.skillId = i + 1; //슬롯 번호 가져오는거랑 똑같은데 + 1 한 이유는 지금 0번이 평타 고정이라 1부터 시작해야 함
                }
            }

            //이 아래는 스킬을 선택했을 때 다른 슬롯에 이미 그 스킬이 있는지 확인하고 예외처리 하는 부분
            //만약 이미 그 스킬이 다른 슬롯에 존재한다면 서로 위치를 바꿔야 함
            int curSlotChar = userData.skillSlot[slotId]; //지금 선택한 슬롯에 어떤 스킬이 있는지 가져오기
            for (int i = 0; i < userData.skillSlot.Length; i++)
            {
                if (skillData.skillId == userData.skillSlot[i]) //만약 지금 선택한 스킬이 이미 슬롯에 있다면
                {
                    userData.skillSlot[i] = curSlotChar; //지금 선택한 슬롯에 있는 스킬을 거기에 넣기
                    userData.skillSlot[slotId] = skillData.skillId; //그리고 거기 있는 스킬을 지금 선택한 슬롯에 넣기
                }
                else //ㄴㄴ 슬롯에 중복된 스킬 없음
                {
                    userData.skillSlot[slotId] = skillData.skillId; //그럼 예정대로 선택했던 슬롯에 지금 선택한 스킬을 넣기
                }
            }

            SkillInfoChange(); //슬롯의 정보가 바뀌었으니 ui갱신
        }
    }

    //스킬 ui 갱신
    public void SkillInfoChange()
    {
        isSlotClicked = false; //이 함수가 동작하는 구간은 스킬ui 열렸을 때, 슬롯에 스킬을 장착했을 때이기 때문에 슬롯이 클릭되지 않은 상태로 설정
        TextMeshProUGUI[] skillName = new TextMeshProUGUI[skills.Length];

        for (int i = 0; i < skills.Length; i++) //스킬 ui들을 싹 다 갱신 시켜줘야 하는거라 개수만큼 다 돌린거임
        {
            skillName[i] = skills[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            skillName[i].text = dataManager.skillTbl[i + 1]["Desc"].ToString(); //지금 있는 스킬들 이름 보여주기, 마찬가지로 지금 0은 평타라 1부터 보여줘야 하기 때문에 i+1을 씀
        }

        for (int i = 0; i < slotName.Length; i++) //슬롯 ui 갱신하는거, 위에 스킬이랑 같은 기능
        {
            slotName[i].text = dataManager.skillTbl[userData.skillSlot[i]]["Desc"].ToString(); //지금 슬롯에 있는 스킬들 이름 보여주기, 여긴 슬롯에 있는 id 그대로 참조하는거라 i그대로 씀
        }

        //두 반복문을 분리한 이유는, 지금 당장엔 슬롯수 == 스킬수 라서 합쳐도 되지만
        //스킬수는 유저의 진행 상태에 따라 달라지기 때문에 구분해줘야 함

        //스킬이랑 슬롯이랑 둘이 왜 코드가 다르냐?
        //슬롯은 우리가 따로 바꾸지 않는이상 3+3 총 6개일거임, 그래서 우리가 인스펙터에서 직접 연결해주는게 더 나은데
        //유저가 스킬 ui 열때마다 스킬 개수가 매번 다를 수 있기 때문에 우리가 그때그때 연결을 못해줌
        //그래서 그냥 갱신할 때 지금 갱신해야 할 것들 가져와서 해주는거임
    }
}
