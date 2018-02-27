using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settings;

    public void Play()
    {
        SceneManager.LoadScene("Testing", LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadSettingsMenu()
    {
        mainMenu.SetActive(false);
        settings.SetActive(true);
    }

    public void SettingsBackBtn()
    {
        settings.SetActive(false);
        mainMenu.SetActive(true);
    }
}
