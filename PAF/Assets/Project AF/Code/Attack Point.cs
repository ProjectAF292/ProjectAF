using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    public Transform player; // �÷��̾� ������Ʈ
    public float distanceFromPlayer = 1.0f; // �÷��̾�� �ﰢ�� ���� �Ÿ�

    void Update()
    {
        if (player == null) return;

        // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // 2D ȯ�濡���� Z�� 0���� ����

        // �÷��̾� ��ġ ���� ���� ����
        Vector3 direction = (mousePosition - player.position).normalized;

        // �ﰢ�� ��ġ�� �÷��̾�� ���� �Ÿ� ������ ������ ����
        transform.position = player.position + direction * distanceFromPlayer;

        // ȸ�� ���� ��� (������ ������ ��ȯ)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
