using UnityEditor;
using UnityEngine;

public class QuitFromMenu : MonoBehaviour
{
    public void OnQuitButtonClick()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
