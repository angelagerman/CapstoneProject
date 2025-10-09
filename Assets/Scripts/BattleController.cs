using UnityEngine;

public class BattleController : MonoBehaviour
{
    public CameraSwitch CameraSwitch;

    public Camera overworldCamera;
    public Camera battleCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            EndBattle();
        }
    }

    public void StartBattle()
    {
        CameraSwitch.SwapActiveCamera(overworldCamera, battleCamera);
    }

    void EndBattle()
    {
        CameraSwitch.SwapActiveCamera(battleCamera, overworldCamera);
    }
}
