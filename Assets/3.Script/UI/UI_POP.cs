using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_POP : MonoBehaviour
{
    [SerializeField] private GameObject btn_panel;
    private Text stage_text;
    private void SetText()
    {
        if (GameManager.instance.current_scene == null) return;

        if (GameManager.instance.current_scene == "EasyGame")
        {
            stage_text.text = "�ʺ� �������� �����\n�����˷� 38�⵿�� �������� �˼�\n������� Ż���� ������ ����մϴ�.";
        }
        else
        {
            stage_text.text = "������� Ż���� ���Ƴ� �����\nŻ���� ���� ��â���� �������\n�������� �����մϴ�.";
        }
    }
    private void Start()
    {
        gameObject.SetActive(false);
        stage_text = GetComponentInChildren<Text>();
    }
    private void OnEnable()
    {
        if (GameManager.instance!=null&& GameManager.instance.current_scene != null)
        {
            SetText();
        }
    }
    public void SceneLoad()
    {
        SceneManager.LoadScene(GameManager.instance.current_scene);
    }
    public void Disable()
    {
        btn_panel.SetActive(true);
        gameObject.SetActive(false);
    }
}
