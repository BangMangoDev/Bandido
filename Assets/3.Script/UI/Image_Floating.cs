using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Image_Floating : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform canvasRectTransform; // Canvas의 RectTransform
    private RectTransform imageRectTransform; // UI Image의 RectTransform
    public float moveSpeed = 100f; // UI Image의 이동 속도
    private Vector2 movementDirection; // 이동할 방향
    private AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        imageRectTransform = GetComponent<RectTransform>();
        StartCoroutine(ChangeDirectionRandomly()); // 일정 시간마다 랜덤하게 방향 변경
    }

    private void Update()
    {
        MoveImage();
        KeepImageWithinCanvas();
    }

    // 이미지가 Canvas 안에서 계속 움직이게 하기
    private void MoveImage()
    {
        imageRectTransform.anchoredPosition += movementDirection * moveSpeed * Time.deltaTime;
    }

    // 이미지가 Canvas 안에 있도록 위치 제한
    private void KeepImageWithinCanvas()
    {
        Vector2 anchoredPosition = imageRectTransform.anchoredPosition;

        // Canvas의 경계 내에서만 움직이도록 제한
        float halfWidth = canvasRectTransform.rect.width / 2 - imageRectTransform.rect.width / 2;
        float halfHeight = canvasRectTransform.rect.height / 2 - imageRectTransform.rect.height / 2;

        if (anchoredPosition.x > halfWidth || anchoredPosition.x < -halfWidth)
        {
            movementDirection.x = -movementDirection.x; // X축 반전
        }
        if (anchoredPosition.y > halfHeight || anchoredPosition.y < -halfHeight)
        {
            movementDirection.y = -movementDirection.y; // Y축 반전
        }

        // 이미지의 위치를 경계 내로 유지
        anchoredPosition.x = Mathf.Clamp(anchoredPosition.x, -halfWidth, halfWidth);
        anchoredPosition.y = Mathf.Clamp(anchoredPosition.y, -halfHeight, halfHeight);

        imageRectTransform.anchoredPosition = anchoredPosition;
    }

    // 일정 간격으로 방향을 랜덤하게 바꿔주는 코루틴
    private IEnumerator ChangeDirectionRandomly()
    {
        while (true)
        {
            movementDirection = Random.insideUnitCircle.normalized; // 랜덤한 방향 설정
            yield return new WaitForSeconds(Random.Range(0.5f, 2f)); // 랜덤한 시간 동안 방향 유지
        }
    }

    // 마우스가 UI Image 위에 있을 때 새로운 위치로 이동
    public void OnPointerEnter(PointerEventData eventData)
    {
        audio.PlayOneShot(audio.clip);
        SetRandomPosition();
    }

    // UI Image의 위치를 Canvas 안에서 랜덤하게 이동시키는 메서드
    private void SetRandomPosition()
    {
        float halfWidth = canvasRectTransform.rect.width / 2 - imageRectTransform.rect.width / 2;
        float halfHeight = canvasRectTransform.rect.height / 2 - imageRectTransform.rect.height / 2;

        // Canvas 내에서의 랜덤 위치 설정
        float randomX = Random.Range(-halfWidth, halfWidth);
        float randomY = Random.Range(-halfHeight, halfHeight);

        imageRectTransform.anchoredPosition = new Vector2(randomX, randomY);

        // 새로운 방향으로 이동
        movementDirection = Random.insideUnitCircle.normalized;
    }

    // 마우스 클릭 시 처리할 로직 (현재는 비어있음)
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Image Clicked");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 마우스가 이미지에서 벗어났을 때 처리할 로직 (현재는 비어있음)
    }
}
