using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiddleQuestScript : MonoBehaviour
{


    [SerializeField] private bool IsObtainable = false;
    public int questItemCount = 0;
    public int riddleItemCount = 0;
    public int badQuestItemCount = 0;
    public int badRiddleItemCount = 0;

    [SerializeField] QuestLog questLog;

    [SerializeField] private AudioSource source;

    public List<GameObject> riddleQuestItems;

    [SerializeField] List<GameObject> questItems;


    [Header("Particals")]
    [SerializeField] private ParticleSystem CollisionParticleSystem;
    public bool once = true;


    void Start()
    {
        // set the quest items inactive until the layer has received the quest from the NPC
        foreach (GameObject riddleQuestItem in riddleQuestItems)
        {
            riddleQuestItem.SetActive(IsObtainable);
        }
        foreach (GameObject questItem in questItems)
        {
            questItem.SetActive(IsObtainable);
        }
    }
    public void increaseQuestItemCount()
    {
        questItemCount++;
    }
    public void increaseRiddleItemCount()
    {
        riddleItemCount++;
    }
    public void increaseBadQuestItemCount()
    {
        badQuestItemCount++;
    }
    public void increaseBadRiddleItemCount()
    {
        badRiddleItemCount++;
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        //if player collides with the QuestItem and increment a counter
        if (collision.gameObject.CompareTag("QuestItem"))
        {
            source.Play();
            increaseQuestItemCount();
            questLog.UpdateQuestProgress(questItemCount);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("RiddleQuestItem"))
        {

            
            source.Play();
            increaseRiddleItemCount();
            questLog.UpdateRiddleQuestProgress();
            Destroy(collision.gameObject);
        }
    }
    public void ChangeIsObtainableToTrue()
    {
        IsObtainable = true;
        foreach (GameObject riddleQuestItem in riddleQuestItems)
        {
            riddleQuestItem.SetActive(IsObtainable);
        }
        foreach (GameObject questItem in questItems)
        {
            questItem.SetActive(IsObtainable);
        }
    }

    void DestroyObj(GameObject GameObj){
        Destroy(GameObj);
    }


}
