using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLog : MonoBehaviour
{
    [Header("Child Objects")]
    [SerializeField] RectTransform questItemProgressBar;
    [SerializeField] RectTransform riddleQuestItemProgressBar;
    [SerializeField] GameObject riddleQuestUI;

    [Header("Necessary scripts")]
    [SerializeField] Interactable interactable;

    private void Start()
    {
        if (interactable.RiddleItemsNeeded == 0)
        {
            riddleQuestUI.SetActive(false);
        }
    }

    public void UpdateQuestProgress(int questItemCount)
    {
        float right = Mathf.Clamp(244 - (238 / interactable.QuestItemsNeeded * questItemCount), 6, 244);
        questItemProgressBar.offsetMax = new Vector2(-right, questItemProgressBar.offsetMax.y);
    }

    public void UpdateRiddleQuestProgress()
    {
        float right = 6;
        riddleQuestItemProgressBar.offsetMax = new Vector2(-right, questItemProgressBar.offsetMax.y);
    }
}
