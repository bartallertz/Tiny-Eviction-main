using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] GameObject MovementTutorial;
    [SerializeField] GameObject QuestTutorial;
    [SerializeField] GameObject InventoryTutorial;

    [Header("Other Scripts")]
    [SerializeField] LevelTimer levelTimer;
    [SerializeField] Interactable questNPC;
    [SerializeField] InventoryButton invButton;

    private bool isMovementTutorialOpen;
    private bool isQuestTutorialOpen;
    private bool isInventoryTutorialOpen;

    void Update()
    {
        // Update our private states and the timer
        updateStates();
        updateTimer();

        // Assign listeners for closing screens
        assignListeners();
        

        // Show correct tutorials
        if (PlayerPrefs.HasKey("resetTutorialsJob")){
            // Reset canvas tutorials
            PlayerPrefs.DeleteKey("hasCompletedMovementTutorial");
            PlayerPrefs.DeleteKey("hasCompletedQuestTutorial");
            PlayerPrefs.DeleteKey("hasCompletedInventoryTutorial");
            // Reset level tutorials
            PlayerPrefs.DeleteKey("hasCompletedNPCTutorial");
            PlayerPrefs.DeleteKey("hasCompletedCollectibleTutorial");
            PlayerPrefs.DeleteKey("hasCompletedPowerUpTutorial");
            PlayerPrefs.DeleteKey("hasCompletedTrapTutorial");
            PlayerPrefs.DeleteKey("hasCompletedWaterTutorial");
            // Reset the reset button
            PlayerPrefs.DeleteKey("resetTutorialsJob");
        } else if (!PlayerPrefs.HasKey("hasCompletedMovementTutorial")){
            showMovementTutorial();
        } else if (PlayerPrefs.HasKey("hasCompletedMovementTutorial") 
            && !PlayerPrefs.HasKey("hasCompletedQuestTutorial")
            && questNPC.HasStarted && !questNPC.isInrange){
            showQuestTutorial();
        }
    }

    void showMovementTutorial()
    {
        MovementTutorial.SetActive(true);
    }

    void showQuestTutorial()
    {
        QuestTutorial.SetActive(true);
    }

    public void showInventoryTutorial()
    {
        InventoryTutorial.SetActive(true);
    }

    void listenForTouch(GameObject tutorialOverlay, string tutorialName)
    {
        if (Input.touchCount != 0){
            if (Input.GetTouch(0).phase == TouchPhase.Began){
                tutorialOverlay.SetActive(false);
                PlayerPrefs.SetInt("hasCompleted" + tutorialName, 1);
                levelTimer.isTimerPaused = false;
                if (tutorialName.Equals("InventoryTutorial")){
                    invButton.ToggleInventoryPanel();
                }
            }
        }
    }

    void updateStates()
    {
        isMovementTutorialOpen = MovementTutorial.activeSelf;
        isQuestTutorialOpen = QuestTutorial.activeSelf;
        isInventoryTutorialOpen = InventoryTutorial.activeSelf;
        
    }

    void updateTimer()
    {
        if (isMovementTutorialOpen || isQuestTutorialOpen || isInventoryTutorialOpen) {
            levelTimer.isTimerPaused = true;
        }
    }

    void assignListeners()
    {
        if (isMovementTutorialOpen){
            listenForTouch(MovementTutorial, nameof(MovementTutorial));
        }
        if (isQuestTutorialOpen){
            listenForTouch(QuestTutorial, nameof(QuestTutorial));
        }
        if (isInventoryTutorialOpen){
            listenForTouch(InventoryTutorial, nameof(InventoryTutorial));
        }
    }
    
}
