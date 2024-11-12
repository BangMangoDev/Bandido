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
            stage_text.text = "초보 교도관인 당신은\n절도죄로 38년동안 수감중인 죄수\n장발장의 탈옥을 막으러 출발합니다.";
        }
        else
        {
            stage_text.text = "장발장의 탈옥을 막아낸 당신은\n탈옥의 귀재 신창원을 잡기위해\n고군분투를 시작합니다.";
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
