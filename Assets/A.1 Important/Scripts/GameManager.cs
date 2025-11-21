using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public static bool isPaused = false;

    public GameObject pauseMenuUI;
    public GameObject pausePanel;
    public GameObject settingsPanel;
    public GameObject partyPanel;
    public GameObject teamSelectPanel;
    public Button saveButton;
    public Button partyConfigButton;

    public Toggle autoRunToggle;
    public Slider mouseSensitivitySlider;
    
    public CameraController cameraController;
    public PlayerController playerController;
    
    public CharacterStats[] currentStats;
    public const string AUTO_RUN_KEY = "AutoRunEnabled";
    public const string MOUSE_SENS_KEY = "MouseSensitivity";

    private void Start()
    {
        playerController.isAutoRunEnabled = PlayerPrefs.GetInt(AUTO_RUN_KEY, playerController.isAutoRunEnabled ? 1 : 0) == 1;
        cameraController.mouseSensitivity = PlayerPrefs.GetFloat(MOUSE_SENS_KEY, cameraController.mouseSensitivity);
        
        autoRunToggle.isOn = playerController.isAutoRunEnabled;
        autoRunToggle.onValueChanged.AddListener(OnToggleAutorunChanged);
        
        mouseSensitivitySlider.value = cameraController.mouseSensitivity;
        mouseSensitivitySlider.onValueChanged.AddListener(OnMouseSensitivityChanged);
    }

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
        saveButton.interactable = !playerController.isInCombat;
        partyConfigButton.interactable = !playerController.isInCombat;

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

    public void OpenPartyMenu()
    {
        if (partyPanel) partyPanel.SetActive(true);
        if (pausePanel) pausePanel.SetActive(false);
    }

    public void OpenTeamMenu()
    {
        if (teamSelectPanel) teamSelectPanel.SetActive(true);
        if (partyPanel) partyPanel.SetActive(false);
    }
    
    public void BackToPauseMenu()
    {
        if (settingsPanel) settingsPanel.SetActive(false);
        if (partyPanel) partyPanel.SetActive(false);
        if (pausePanel) pausePanel.SetActive(true);
    }

    public void BackToPartyMenu()
    {
        if (teamSelectPanel) teamSelectPanel.SetActive(false);
        if (partyPanel) partyPanel.SetActive(true);
    }

    public void OnQuitButtonClick()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    public void SaveGame()
    {
        foreach (var stats in currentStats)
        {
            SaveSystem.SaveCharacter(stats);
        }

        Debug.Log("Game Saved!");
    }
    
    void OnToggleAutorunChanged(bool isOn)
    {
        playerController.isAutoRunEnabled = isOn;
        PlayerPrefs.SetInt(AUTO_RUN_KEY, isOn ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    void OnMouseSensitivityChanged(float value)
    {
        cameraController.mouseSensitivity = value;
        PlayerPrefs.SetFloat(MOUSE_SENS_KEY, value);
        PlayerPrefs.Save();
    }

    public void ReturnToStartMenu()
    {
        SceneManager.LoadScene("StartScene");
    }
}
