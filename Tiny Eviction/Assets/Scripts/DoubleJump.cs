using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MonoBehaviour
{
    public PlayerMovement script;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            script.playerCanDoubleJump = true;
            this.gameObject.SetActive(false);
        }
    }
}
