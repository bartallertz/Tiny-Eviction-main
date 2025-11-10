using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsController : MonoBehaviour
{

    [Header("Game Objects")]
    [SerializeField] GameObject pausePopup;
    [SerializeField] Toggle fpsToggle;
    [SerializeField] Toggle levelToggle;
    [SerializeField] Toggle itemToggle;
    [SerializeField] GameObject advancedSettingsContainer;
    [SerializeField] TMP_InputField cheatInput;
    [SerializeField] TMP_Text cheatErrorField;

    [Header("Scripts")]
    [SerializeField] LevelTimer levelTimer;

    private bool advancedSettingsRendered = false;

    void Start()
    {
        assignValues();
    }

    void Update() {
        assignValues();
        if (!advancedSettingsRendered && PlayerPrefs.HasKey("hasUsedExtendedSettingsCheat")){
            showAdvancedSettings();
        }
    }

    public void openPausePopup() {
        if (levelTimer){
            levelTimer.isTimerPaused = true;
        }
        pausePopup.SetActive(true);
    }

    public void closePausePopup() {
        if (levelTimer){
            levelTimer.isTimerPaused = false;
        }
        pausePopup.SetActive(false);
    }

    public void assignValues(){
        fpsToggle.isOn = PlayerPrefs.HasKey("isFPSCounterEnabled");
        levelToggle.isOn = PlayerPrefs.HasKey("hasAllLevelsUnlocked");
        itemToggle.isOn = PlayerPrefs.HasKey("hasAllItemsUnlocked");
    }

    public void toggleFPSCounter(Toggle toggle) {
        if (toggle.isOn){
            PlayerPrefs.SetInt("isFPSCounterEnabled", 1);
        } else {
            PlayerPrefs.DeleteKey("isFPSCounterEnabled");
        }
    }

    public void toggleLevelUnlock(Toggle toggle) {
        if (toggle.isOn) {
            PlayerPrefs.SetInt("hasAllLevelsUnlocked", 1);
        } else {
            PlayerPrefs.DeleteKey("hasAllLevelsUnlocked");
        }
    }

    public void toggleEquipmentUnlock(Toggle toggle) {
        if (toggle.isOn) {
            PlayerPrefs.SetInt("hasAllItemsUnlocked", 1);
        } else {
            PlayerPrefs.DeleteKey("hasAllItemsUnlocked");
        }
    }

    public void validateCheat() {
        string cheatText = cheatInput.text;
        switch (cheatText)
        {
            case string input when (cheatText == null || cheatText.Equals("")):
                showCheatMessage(false, "Please enter a cheat!");
                break;
            case string input when (cheatText.Equals("LetsHelpTinyTimmy")):
                showCheatMessage(true, "Yay, cheat successful! Unlocked advanced settings!");
                PlayerPrefs.SetInt("hasUsedExtendedSettingsCheat", 1);
                break;
            default: 
                showCheatMessage(false, "Aw. Unknown cheat, please try again!");
                break;
        }
    }

    public void showCheatMessage(bool isSuccess, string message){
        cheatErrorField.gameObject.SetActive(true);
        cheatErrorField.SetText(message);
        if (isSuccess) {
            cheatErrorField.color = new Color(0.08f, 0.7f, 0.32f, 1.0f); // Green
        } else {
            cheatErrorField.color = new Color(0.9f, 0.1f, 0.1f, 1.0f); // Red
        }
    }

    public void showAdvancedSettings(){
        advancedSettingsRendered = true;
        advancedSettingsContainer.SetActive(true);
    }
}
