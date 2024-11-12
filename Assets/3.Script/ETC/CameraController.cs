using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float zoomSpeed = 10f;        // 마우스 휠 줌 속도
    private float panSpeed = 0.5f;        // 마우스 휠 클릭 후 이동 속도
    private float minY = -3f;              // 카메라의 최소 Y 위치
    private float maxY = 50f;             // 카메라의 최대 Y 위치

    private Vector3 lastMousePosition;   // 마지막 마우스 위치 저장

    void Update()
    {
        // 1. 마우스 휠로 Y 값 조정 (줌 인/아웃)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            Vector3 position = transform.position;
            position.y -= scroll * zoomSpeed;
            position.y = Mathf.Clamp(position.y, minY, maxY); // 최소, 최대 Y 값 제한
            transform.position = position;
        }

        // 2. 마우스 휠 클릭 후 X, Z 값 조정 (이동)
        if (Input.GetMouseButtonDown(2)) // 마우스 휠 버튼 클릭 시
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2)) // 마우스 휠 버튼을 클릭한 상태로 마우스를 이동할 때
        {
            Vector3 deltaMousePosition = Input.mousePosition - lastMousePosition; // 마우스 이동 거리 계산
            Vector3 position = transform.position;

            // 마우스 이동 거리에 따라 카메라의 X, Z 위치를 변경
            position.x -= deltaMousePosition.x * panSpeed * Time.deltaTime;
            position.z -= deltaMousePosition.y * panSpeed * Time.deltaTime;

            transform.position = position;
            lastMousePosition = Input.mousePosition; // 마지막 마우스 위치 갱신
        }
    }
}
