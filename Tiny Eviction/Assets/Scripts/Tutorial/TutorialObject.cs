using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialObject : MonoBehaviour
{

    [Header("Tutorial Data")]
    [SerializeField] tutorialType typeOfTutorial;

    [Header("Always required: Game Objects")]
    [SerializeField] GameObject tutorialContainer;
    [SerializeField] TMP_Text tutorialText;

    [Header("Stuff needed for NPC Tutorial")]
    [SerializeField] string NPCName;
    [SerializeField] Interactable questNPC;

    [Header("Stuff needed for Collectible Tutorial")]
    [SerializeField] GameObject collectibleItem;
    [SerializeField] Interactable currentQuest;
    [SerializeField] RiddleQuestScript riddleQuest;

    [Header("Stuff needed for PowerUp Tutorial")]
    [SerializeField] GameObject powerUpItem;

    [Header("Stuff needed for Trap Tutorial")]
    [SerializeField] Traps trapObject;

    public enum tutorialType{NPC, Collectible, PowerUp, Trap, Water};

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.HasKey("hasCompletedMovementTutorial")){
            switch (typeOfTutorial){
                case tutorialType type when (type == tutorialType.NPC):
                    displayNPCTutorial();
                    break;
                case tutorialType type when (type == tutorialType.Collectible):
                    displayCollectibleTutorial();
                    break;
                case tutorialType type when (type == tutorialType.PowerUp):
                    displayPowerUpTutorial();
                    break;
                case tutorialType type when (type == tutorialType.Trap):
                    displayTrapTutorial();
                    break;
                case tutorialType type when (type == tutorialType.Water):
                    break;
                default:
                    break;
        }
        }
    }

    void displayNPCTutorial(){
        if (questNPC != null){
            if (!questNPC.HasStarted && !PlayerPrefs.HasKey("hasCompletedNPCTutorial")) {
                tutorialContainer.SetActive(true);
                if (NPCName == null || NPCName.Equals("")){
                    NPCName = "an NPC";
                }
                tutorialText.SetText(
                    "Looks like you have encountered " 
                    + NPCName 
                    +". They look welcoming! Approach them, and you might have a place to stay tonight.");
                PlayerPrefs.SetInt("hasCompletedNPCTutorial", 1);
            }
        }
        if (tutorialContainer.activeSelf && PlayerPrefs.HasKey("hasCompletedNPCTutorial") && questNPC.HasStarted){
            tutorialContainer.SetActive(false);
        }
    }

    void displayCollectibleTutorial(){
        if (collectibleItem != null){
            if (currentQuest.HasStarted && !PlayerPrefs.HasKey("hasCompletedCollectibleTutorial")) {
                tutorialContainer.SetActive(true);
                tutorialText.SetText("You have spotted a collectible! I recall the NPC needing this. Just be careful, only pick up the nice and clean items.");
            } else if (
                riddleQuest.badQuestItemCount > 0
                || riddleQuest.badRiddleItemCount > 0
                || riddleQuest.questItemCount > 0
                || riddleQuest.riddleItemCount > 0
            ) {
                tutorialContainer.SetActive(false);
                PlayerPrefs.SetInt("hasCompletedCollectibleTutorial", 1);
            }
        }
        else {
            tutorialContainer.SetActive(false);
            PlayerPrefs.SetInt("hasCompletedCollectibleTutorial", 1);
        }
    }

    void displayPowerUpTutorial(){
        if (powerUpItem != null && !PlayerPrefs.HasKey("hasCompletedPowerUpTutorial")){
            tutorialContainer.SetActive(true);
            tutorialText.SetText("You have spotten a Power Up hat! Pick this up to gain an awesome, helpful ability!");
        } else {
            tutorialContainer.SetActive(false);
            PlayerPrefs.SetInt("hasCompletedPowerUpTutorial", 1);
        }
    }

    void displayTrapTutorial(){
        if (trapObject && !PlayerPrefs.HasKey("hasCompletedTrapTutorial")) {
            tutorialContainer.SetActive(true);
            tutorialText.SetText("This is a trap! Be careful - traps hurt Timmy, making us lose precious daylight. There are multiple different kinds of traps, so beware!");
        } else {
            tutorialContainer.SetActive(false);
        }
    }
}
