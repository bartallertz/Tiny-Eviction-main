using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButton : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] GameObject inventoryScreen;

    [Header("Scripts")]
    [SerializeField] LevelTimer timer;
    [SerializeField] TutorialController tutorialController;

    public void ToggleInventoryPanel()
    {
        if (PlayerPrefs.HasKey("hasCompletedInventoryTutorial")){
            inventoryScreen.SetActive(!inventoryScreen.activeSelf);
            if (inventoryScreen.activeSelf == true && timer)
            {
                timer.isTimerPaused = true;
            }
            else if (inventoryScreen.activeSelf == false && timer)
            {
                timer.isTimerPaused = false;
            }
        } else {
            tutorialController.showInventoryTutorial();
        }
    }
}
