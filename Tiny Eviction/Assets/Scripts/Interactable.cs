using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    [Header("Dialog")]
    public GameObject DialoguePanel;

    public TextMeshPro DialogueText;
    [SerializeField] private bool IsDialogueScreenActive = false;

    public string[] Dialog;
    [SerializeField] private int index;

    public float wordspeed;


    [Header("Quest")]
    public RiddleQuestScript PlayerObject;

    public LevelTimer Timer;

    //needed quest items to advance
    public int QuestItemsNeeded;
    private int QuestCount;

    //needed riddle items to advance
    public int RiddleItemsNeeded;
    private int RiddleCount;

    public bool isInrange = false;

    [SerializeField] public bool HasStarted = false;

    [SerializeField] private PlayerInventory playerInventory;

    [SerializeField] GameObject GameCompletedObject;

    public bool typing = false;
    private bool loadNextLevel = false;

    // Update is called once per frame
    void Update()
    {
        //quest items you have collected
        QuestCount = PlayerObject.questItemCount;
        // riddle items you have collected
        RiddleCount = PlayerObject.riddleItemCount;

        //Start quest if not already started
        if (isInrange && (QuestCount <= QuestItemsNeeded && RiddleCount <= RiddleItemsNeeded) && HasStarted == false && IsDialogueScreenActive == false)
        {
            if (DialoguePanel.activeInHierarchy)
            {
                ZeroText();
            }
            else
            {
                DialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }


            Timer.isTimerPaused = false;
            HasStarted = true;
            IsDialogueScreenActive = true;
            PlayerObject.ChangeIsObtainableToTrue();
            index++;


        }
        //quest has been started and you need to give the NPC items
        if (isInrange && (QuestCount < QuestItemsNeeded || RiddleCount < RiddleItemsNeeded) && HasStarted == true && IsDialogueScreenActive == false)
        {

            if (DialoguePanel.activeInHierarchy)
            {
                ZeroText();
            }
            else
            {
                index = 1;
                DialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
            IsDialogueScreenActive = true;
        }
        //Quest completed
        if (isInrange && (QuestCount >= QuestItemsNeeded && RiddleCount >= RiddleItemsNeeded) && HasStarted == true && IsDialogueScreenActive == false)
        {
            index = 2;
            if (DialoguePanel.activeInHierarchy)
            {
                ZeroText();
            }
            else
            {
                DialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
            IsDialogueScreenActive = true;
            CompleteLevel();
        }

        if (loadNextLevel && !typing)
        {
            if (SceneManager.GetActiveScene().buildIndex < 7) {
                Invoke("LoadLevelSelect", 3);
            } else {
                Invoke("CompleteGame", 3);
            }
        }
    }

    IEnumerator Typing()
    {
        typing = true;

        foreach (var letter in Dialog[index].ToCharArray())
        {
            if (isInrange)
            {
                DialogueText.text += letter;

                yield return new WaitForSeconds(wordspeed);
            }
            else
            {
                break;
            }

        }
        typing = false;

    }
    public void NextLine()
    {
        if (index < Dialog.Length - 1)
        {
            index++;
            DialogueText.text = string.Empty;
            StartCoroutine(Typing());
        }
        else
        {
            ZeroText();
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInrange = true;

        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ZeroText();
            isInrange = false;
            IsDialogueScreenActive = false;
            DialogueText.text = string.Empty;
        }
    }
    private void CompleteLevel()
    {
        PlayerPrefs.SetInt("Lv" + (SceneManager.GetActiveScene().buildIndex), 1);
        playerInventory.activeInventory.Save();
        loadNextLevel = true;
    }

    private void CompleteGame()
    {
        GameCompletedObject.SetActive(true);
    }

    private void LoadLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void ZeroText()
    {
        DialogueText.text = string.Empty;
        DialoguePanel.SetActive(false);
    }
}
