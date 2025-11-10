using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{

    public LevelTimer levelTimer;

    [SerializeField] private AudioSource AudioSource;

    [SerializeField] Animator bearTrapAnimator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (bearTrapAnimator != null)
            {
                bearTrapAnimator.SetBool("playerInRange", true);
                BoxCollider2D bearTrapCollider = GetComponent<BoxCollider2D>();
                bearTrapCollider.enabled = false;
            }
            AudioSource.Play();
            levelTimer.currentTime -= 10f;
            PlayerPrefs.SetInt("hasCompletedTrapTutorial", 1);
            PlayerAnimation playerAnim = collision.GetComponentInChildren<PlayerAnimation>();
            playerAnim.isHurt = true;
        }
    }


}
