using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float zoomSpeed = 10f;        // ���콺 �� �� �ӵ�
    private float panSpeed = 0.5f;        // ���콺 �� Ŭ�� �� �̵� �ӵ�
    private float minY = -3f;              // ī�޶��� �ּ� Y ��ġ
    private float maxY = 50f;             // ī�޶��� �ִ� Y ��ġ

    private Vector3 lastMousePosition;   // ������ ���콺 ��ġ ����

    void Update()
    {
        // 1. ���콺 �ٷ� Y �� ���� (�� ��/�ƿ�)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            Vector3 position = transform.position;
            position.y -= scroll * zoomSpeed;
            position.y = Mathf.Clamp(position.y, minY, maxY); // �ּ�, �ִ� Y �� ����
            transform.position = position;
        }

        // 2. ���콺 �� Ŭ�� �� X, Z �� ���� (�̵�)
        if (Input.GetMouseButtonDown(2)) // ���콺 �� ��ư Ŭ�� ��
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2)) // ���콺 �� ��ư�� Ŭ���� ���·� ���콺�� �̵��� ��
        {
            Vector3 deltaMousePosition = Input.mousePosition - lastMousePosition; // ���콺 �̵� �Ÿ� ���
            Vector3 position = transform.position;

            // ���콺 �̵� �Ÿ��� ���� ī�޶��� X, Z ��ġ�� ����
            position.x -= deltaMousePosition.x * panSpeed * Time.deltaTime;
            position.z -= deltaMousePosition.y * panSpeed * Time.deltaTime;

            transform.position = position;
            lastMousePosition = Input.mousePosition; // ������ ���콺 ��ġ ����
        }
    }
}
