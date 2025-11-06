using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static bool isPaused = false;

    public GameObject pauseMenuUI;
    public GameObject pausePanel;
    public GameObject settingsPanel;
    public Button[] menuButtons; // Array to hold all buttons in the pause menu
    private int selectedButtonIndex = 0; // To track the currently selected button
    private EventSystem eventSystem;
    
    void Start()
    {
        eventSystem = EventSystem.current;
        if (menuButtons.Length > 0)
        {
            SelectButton(selectedButtonIndex); // Initially select the first button
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        
        if (isPaused)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) // Move up
            {
                selectedButtonIndex = Mathf.Max(selectedButtonIndex - 1, 0); // Stay within bounds
                SelectButton(selectedButtonIndex);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) // Move down
            {
                selectedButtonIndex = Mathf.Min(selectedButtonIndex + 1, menuButtons.Length - 1); // Stay within bounds
                SelectButton(selectedButtonIndex);
            }

            if (Input.GetKeyDown(KeyCode.Return)) // Select button
            {
                menuButtons[selectedButtonIndex].onClick.Invoke(); // Click the selected button
            }
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
        }
        else
        {
            Time.timeScale = 1f; // Resume normal time
            if (pauseMenuUI) pauseMenuUI.SetActive(false);
        }
    }
    
    void SelectButton(int index)
    {
        // Deselect all buttons and then select the new one
        eventSystem.SetSelectedGameObject(menuButtons[index].gameObject);
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
}
