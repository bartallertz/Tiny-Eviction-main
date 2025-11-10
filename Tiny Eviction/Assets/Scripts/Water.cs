using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    
    public LevelTimer levelTimer;

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.CompareTag("Player")){
            levelTimer.currentTime = 0;
            levelTimer.GameOverSequence();
        }
    }
}
