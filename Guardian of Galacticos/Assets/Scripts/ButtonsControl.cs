using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsControl : MonoBehaviour
{

    public GameObject howToPlayPanel;
    public GameObject pausePanel;
    public void PlayButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void How_to_Play_Button()
    {
        howToPlayPanel.SetActive(true);
    }

    public void backButton()
    {        
        howToPlayPanel.SetActive(false);
    }

    public void mainMenuButton()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void continueButton()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void restartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Time.timeScale == 1f)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 0f;
                pausePanel.SetActive(true);
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 1f;
                pausePanel.SetActive(false);
            }
        }


    }

}
