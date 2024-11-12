using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set_UI : MonoBehaviour
{
    [SerializeField] private GameObject audio_ui;
    public void AudioSettingOn()
    {
        if (audio_ui.activeSelf) return;
        audio_ui.SetActive(true);
        gameObject.SetActive(false);
    }
    public void AudioSettingOff()
    {
        audio_ui.SetActive(false);
        gameObject.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
