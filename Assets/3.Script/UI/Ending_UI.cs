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

            ending_text.text = "당신은 성공적으로 탈옥수\n\n장발장을 잡는데 성공했습니다!\n\n이제 그는 죽을 때가지 빛을 보지 못하고\n\n고독하고 쓸쓸하게 독방에서 늙어 죽게 될 것 입니다.";
            }
            else
            {
                ending_text.text = "신창원까지 무사히 잡아낸\n\n당신은 이제 탈옥수의 전설\n\n더 이상 당신이 못잡을 죄수는 없습니다.";
            }
        }
        else
        {
            if (GameManager.instance.current_scene == "EasyGame")
            {
            ending_text.text = "당신은 탈옥수 장발장을 놓쳤습니다!\n\n이제 당신이 관리소홀과 업무태만으로\n\n그의 자리를 대신하게 되었습니다.\n\n축하드립니다!";
            }
            else
            {
                ending_text.text = "신창원은 당신을 농락하며\n\n무사히 빠져나갔습니다.";
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
