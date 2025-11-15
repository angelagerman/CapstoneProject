using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public static bool isPaused = false;

    public GameObject pauseMenuUI;
    public GameObject pausePanel;
    public GameObject settingsPanel;

    public PlayerController playerController;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // Freeze everything that runs with Time.deltaTime
            //I keep reading deltaTime as deltaRune someone help I think I have a problem
            if (pauseMenuUI) pauseMenuUI.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f; // Resume normal time
            if (pauseMenuUI) pauseMenuUI.SetActive(false);
            if (playerController.isInCombat == false)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    public void OpenSettingsMenu()
    {
        if (settingsPanel) settingsPanel.SetActive(true);
        if (pausePanel) pausePanel.SetActive(false);
    }
    
    public void BackToPauseMenu()
    {
        if (settingsPanel) settingsPanel.SetActive(false);
        if (pausePanel) pausePanel.SetActive(true);
    }

    public void OnQuitButtonClick()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
