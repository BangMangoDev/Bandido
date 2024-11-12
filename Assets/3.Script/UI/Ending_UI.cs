using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ending_UI : MonoBehaviour
{
    [SerializeField]private Text ending_text;
    private void Start()
    {
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        if (GameManager.instance.ending == null)
        {
            return;
        }else if (GameManager.instance.ending == "Win")
        {
            if (GameManager.instance.current_scene == "EasyGame")
            {

            ending_text.text = "����� ���������� Ż����\n\n������� ��µ� �����߽��ϴ�!\n\n���� �״� ���� ������ ���� ���� ���ϰ�\n\n���ϰ� �����ϰ� ���濡�� �ľ� �װ� �� �� �Դϴ�.";
            }
            else
            {
                ending_text.text = "��â������ ������ ��Ƴ�\n\n����� ���� Ż������ ����\n\n�� �̻� ����� ������ �˼��� �����ϴ�.";
            }
        }
        else
        {
            if (GameManager.instance.current_scene == "EasyGame")
            {
            ending_text.text = "����� Ż���� ������� ���ƽ��ϴ�!\n\n���� ����� ������Ȧ�� �����¸�����\n\n���� �ڸ��� ����ϰ� �Ǿ����ϴ�.\n\n���ϵ帳�ϴ�!";
            }
            else
            {
                ending_text.text = "��â���� ����� ����ϸ�\n\n������ �����������ϴ�.";
            }
        }
    }

    public void LoadIntro()
    {
        GameManager.instance.current_scene = null;
        SceneManager.LoadScene("Intro");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
