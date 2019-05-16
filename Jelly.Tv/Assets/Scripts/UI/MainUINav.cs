using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUINav : MonoBehaviour
{
    [SerializeField]
    private GameObject m_mainMenu = null;
    [SerializeField]
    private GameObject m_pauseMenu = null;
    [SerializeField]
    private GameObject m_gameUI = null;

    public void Start()
    {
        TurnOnMainMenu();
    }

    private void Update()
    {
        //we should allow a streamer to set the hotkey later or set it ourselves?
        //for now = (i've never seen it as a hotkey in a game)
        if(Input.GetKeyDown(KeyCode.Equals))
        {
            if(m_pauseMenu.activeInHierarchy) TurnOffPause();
            else TurnOnPause();
        }
    }

    public void TurnOnPause()
    {
        m_pauseMenu.SetActive(true);
    }

    public void TurnOffPause()
    {
        m_pauseMenu.SetActive(false);
    }

    public void TurnOnMainMenu()
    {
        m_pauseMenu.SetActive(false);
        m_gameUI.SetActive(false);
        m_mainMenu.SetActive(true);
    }

    public void TurnOffMainMenu()
    {
        m_mainMenu.SetActive(false);
        m_gameUI.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
