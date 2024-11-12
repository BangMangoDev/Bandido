using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Image_Floating : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform canvasRectTransform; // Canvas�� RectTransform
    private RectTransform imageRectTransform; // UI Image�� RectTransform
    public float moveSpeed = 100f; // UI Image�� �̵� �ӵ�
    private Vector2 movementDirection; // �̵��� ����
    private AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        imageRectTransform = GetComponent<RectTransform>();
        StartCoroutine(ChangeDirectionRandomly()); // ���� �ð����� �����ϰ� ���� ����
    }

    private void Update()
    {
        MoveImage();
        KeepImageWithinCanvas();
    }

    // �̹����� Canvas �ȿ��� ��� �����̰� �ϱ�
    private void MoveImage()
    {
        imageRectTransform.anchoredPosition += movementDirection * moveSpeed * Time.deltaTime;
    }

    // �̹����� Canvas �ȿ� �ֵ��� ��ġ ����
    private void KeepImageWithinCanvas()
    {
        Vector2 anchoredPosition = imageRectTransform.anchoredPosition;

        // Canvas�� ��� �������� �����̵��� ����
        float halfWidth = canvasRectTransform.rect.width / 2 - imageRectTransform.rect.width / 2;
        float halfHeight = canvasRectTransform.rect.height / 2 - imageRectTransform.rect.height / 2;

        if (anchoredPosition.x > halfWidth || anchoredPosition.x < -halfWidth)
        {
            movementDirection.x = -movementDirection.x; // X�� ����
        }
        if (anchoredPosition.y > halfHeight || anchoredPosition.y < -halfHeight)
        {
            movementDirection.y = -movementDirection.y; // Y�� ����
        }

        // �̹����� ��ġ�� ��� ���� ����
        anchoredPosition.x = Mathf.Clamp(anchoredPosition.x, -halfWidth, halfWidth);
        anchoredPosition.y = Mathf.Clamp(anchoredPosition.y, -halfHeight, halfHeight);

        imageRectTransform.anchoredPosition = anchoredPosition;
    }

    // ���� �������� ������ �����ϰ� �ٲ��ִ� �ڷ�ƾ
    private IEnumerator ChangeDirectionRandomly()
    {
        while (true)
        {
            movementDirection = Random.insideUnitCircle.normalized; // ������ ���� ����
            yield return new WaitForSeconds(Random.Range(0.5f, 2f)); // ������ �ð� ���� ���� ����
        }
    }

    // ���콺�� UI Image ���� ���� �� ���ο� ��ġ�� �̵�
    public void OnPointerEnter(PointerEventData eventData)
    {
        audio.PlayOneShot(audio.clip);
        SetRandomPosition();
    }

    // UI Image�� ��ġ�� Canvas �ȿ��� �����ϰ� �̵���Ű�� �޼���
    private void SetRandomPosition()
    {
        float halfWidth = canvasRectTransform.rect.width / 2 - imageRectTransform.rect.width / 2;
        float halfHeight = canvasRectTransform.rect.height / 2 - imageRectTransform.rect.height / 2;

        // Canvas �������� ���� ��ġ ����
        float randomX = Random.Range(-halfWidth, halfWidth);
        float randomY = Random.Range(-halfHeight, halfHeight);

        imageRectTransform.anchoredPosition = new Vector2(randomX, randomY);

        // ���ο� �������� �̵�
        movementDirection = Random.insideUnitCircle.normalized;
    }

    // ���콺 Ŭ�� �� ó���� ���� (����� �������)
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Image Clicked");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // ���콺�� �̹������� ����� �� ó���� ���� (����� �������)
    }
}
