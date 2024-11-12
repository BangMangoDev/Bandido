using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_Select : MonoBehaviour
{
    [SerializeField] private GameObject ui_pop;
    public void StartEasyGame()
    {
        GameManager.instance.current_scene = "EasyGame";
        ui_pop.SetActive(true);
        gameObject.SetActive(false);
    }
    public void StartHardGame()
    {
        GameManager.instance.current_scene = "HardGame";
        ui_pop.SetActive(true);
        gameObject.SetActive(false);
    }
}
