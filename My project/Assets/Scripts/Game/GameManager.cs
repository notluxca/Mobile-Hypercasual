using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public int targetFrameRate = 60;

    void Awake()
    {
        Application.targetFrameRate = targetFrameRate;
        QualitySettings.vSyncCount = 0;
        Debug.Log("FPS locked to " + targetFrameRate);
    }
}
