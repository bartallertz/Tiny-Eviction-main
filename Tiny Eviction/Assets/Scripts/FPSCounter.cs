using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI FPSCounterText;
    [SerializeField] private float counterUpdateRate; // Keep around a second, more often is not needed

    private float currentTimer;
    private bool counterEnabled;

    void Update()
    {
        counterEnabled = PlayerPrefs.HasKey("isFPSCounterEnabled");
        FPSCounterText.enabled = counterEnabled;
        if (counterEnabled) {
            if (Time.unscaledTime  > currentTimer) {
                int currentFPS = (int)(1f / Time.unscaledDeltaTime);
                FPSCounterText.text = string.Format("{0} FPS", currentFPS);
                currentTimer = Time.unscaledTime + counterUpdateRate;
            }
        }
    }
}
