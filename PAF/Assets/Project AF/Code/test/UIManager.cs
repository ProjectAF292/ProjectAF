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
    public GameObject[] skills = new GameObject[3]; //���� ���߿� ������ �ٲ�� ��, List�� ���� Ȱ��ȭ �� ��ų ui �״�� �޾ƿ��� �ؾ� ��
    public GameObject[] skillSlot = new GameObject[3]; //���� ���߿� ���� 6���� �ٲ�� �� �ƴϸ� ����A, B�� ��������
    public TextMeshProUGUI[] slotName = new TextMeshProUGUI[3]; //���⵵ �����̶� �����ϰ� ���� ��
    
    DataManager dataManager;
    UserData userData;

    int slotId;
    bool isSlotClicked;

    private void Start()
    {
        dataManager = DataManager.Instance;
        userData = dataManager.userData;
    }

    //��ų ���� ��ư Ŭ��
    public void SkillChange()
    {
        skillsui.SetActive(!skillsui.activeSelf); //��ųâ ui Ȱ��ȭ/��Ȱ��ȭ �ϱ�, ���� ������ �ݴ�� �ٲ�� ���
        //�� �ؿ� ���� ������ �������� ��ų�� ������ ������ ������ ��ų ui�� Ȱ��ȭ ���Ѿ� �ϱ� ������ �װ� �޾ƿͼ� �迭�� ũ�⸦ �����ϰ�
        //Ȱ��ȭ�� �� ������Ʈ�� ��ų�� ������ ���ʴ�� �־���� ��
        //������ �ϴ� ��ų 3���ۿ� ��� 3���� �س���

        if (skillsui.activeSelf)
        {
            SkillInfoChange(); //��ų ui�� Ȱ��ȭ�Ǿ� ������ ��ųâ ui �����ϴ� �Լ� ȣ��
        }
    }

    //��ų ���� Ŭ��
    public void SlotClicked()
    {
        isSlotClicked = true; //������ Ŭ���� ���¿����� ��ų�� �����Ǿ�� ��, �׷��� ������ Ŭ�� �ƴ��� �Ǵ��ϱ� ���� ������ �ʿ�
        GameObject selectSlot = EventSystem.current.currentSelectedGameObject; //���� Ŭ���� ������Ʈ�� ���� �޾ƿ��� ���
        for (int i = 0; i < skillSlot.Length; i++) //���� �����Ѱ� ������ �´��� Ȯ���Ϸ��� ������ ����ִ� �迭�� �ѹ� �� �Ⱦ�� ��, �׷��� �� ���� ��ŭ �ݺ�
        {
            if (selectSlot == skillSlot[i].gameObject) //���� ������ ������Ʈ�� ������ �´��� Ȯ��
            {
                slotId = i; //���� ������ŭ �ݺ� ������ ���� ���� ������ ������ ������ ���° �������� �޾ƿ���
            }
        }
    }

    //��ų Ŭ��
    public void SkillClicked()
    {
        if (isSlotClicked)
        {
            GameObject selectSkill = EventSystem.current.currentSelectedGameObject; //���������� ���� Ŭ���� ������Ʈ�� ���� �޾ƿ���
            SkillData skillData = selectSkill.GetComponent<SkillData>(); //�׸��� �� ������Ʈ�� ������ �ִ� ��ų ������ ����

            for (int i = 0; i < skills.Length; i++) //���� �κ��̶� ����
            {
                if (selectSkill == skills[i].gameObject) //���� �κ��̶� ����
                {
                    skillData.skillId = i + 1; //���� ��ȣ �������°Ŷ� �Ȱ����� + 1 �� ������ ���� 0���� ��Ÿ �����̶� 1���� �����ؾ� ��
                }
            }

            //�� �Ʒ��� ��ų�� �������� �� �ٸ� ���Կ� �̹� �� ��ų�� �ִ��� Ȯ���ϰ� ����ó�� �ϴ� �κ�
            //���� �̹� �� ��ų�� �ٸ� ���Կ� �����Ѵٸ� ���� ��ġ�� �ٲ�� ��
            int curSlotChar = userData.skillSlot[slotId]; //���� ������ ���Կ� � ��ų�� �ִ��� ��������
            for (int i = 0; i < userData.skillSlot.Length; i++)
            {
                if (skillData.skillId == userData.skillSlot[i]) //���� ���� ������ ��ų�� �̹� ���Կ� �ִٸ�
                {
                    userData.skillSlot[i] = curSlotChar; //���� ������ ���Կ� �ִ� ��ų�� �ű⿡ �ֱ�
                    userData.skillSlot[slotId] = skillData.skillId; //�׸��� �ű� �ִ� ��ų�� ���� ������ ���Կ� �ֱ�
                }
                else //���� ���Կ� �ߺ��� ��ų ����
                {
                    userData.skillSlot[slotId] = skillData.skillId; //�׷� ������� �����ߴ� ���Կ� ���� ������ ��ų�� �ֱ�
                }
            }

            SkillInfoChange(); //������ ������ �ٲ������ ui����
        }
    }

    //��ų ui ����
    public void SkillInfoChange()
    {
        isSlotClicked = false; //�� �Լ��� �����ϴ� ������ ��ųui ������ ��, ���Կ� ��ų�� �������� ���̱� ������ ������ Ŭ������ ���� ���·� ����
        TextMeshProUGUI[] skillName = new TextMeshProUGUI[skills.Length];

        for (int i = 0; i < skills.Length; i++) //��ų ui���� �� �� ���� ������� �ϴ°Ŷ� ������ŭ �� ��������
        {
            skillName[i] = skills[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            skillName[i].text = dataManager.skillTbl[i + 1]["Desc"].ToString(); //���� �ִ� ��ų�� �̸� �����ֱ�, ���������� ���� 0�� ��Ÿ�� 1���� ������� �ϱ� ������ i+1�� ��
        }

        for (int i = 0; i < slotName.Length; i++) //���� ui �����ϴ°�, ���� ��ų�̶� ���� ���
        {
            slotName[i].text = dataManager.skillTbl[userData.skillSlot[i]]["Desc"].ToString(); //���� ���Կ� �ִ� ��ų�� �̸� �����ֱ�, ���� ���Կ� �ִ� id �״�� �����ϴ°Ŷ� i�״�� ��
        }

        //�� �ݺ����� �и��� ������, ���� ���忣 ���Լ� == ��ų�� �� ���ĵ� ������
        //��ų���� ������ ���� ���¿� ���� �޶����� ������ ��������� ��

        //��ų�̶� �����̶� ���� �� �ڵ尡 �ٸ���?
        //������ �츮�� ���� �ٲ��� �ʴ��̻� 3+3 �� 6���ϰ���, �׷��� �츮�� �ν����Ϳ��� ���� �������ִ°� �� ������
        //������ ��ų ui �������� ��ų ������ �Ź� �ٸ� �� �ֱ� ������ �츮�� �׶��׶� ������ ������
        //�׷��� �׳� ������ �� ���� �����ؾ� �� �͵� �����ͼ� ���ִ°���
    }
}
