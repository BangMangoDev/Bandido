using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ingame_UI : MonoBehaviour
{
    public void LoadIntro()
    {
        SceneManager.LoadScene("Intro");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
