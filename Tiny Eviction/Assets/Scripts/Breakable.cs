using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{

    private PlayerMovement player;
    private int hits;
    private Animator treeAnimator;
    private PolygonCollider2D treePolygonCollider;
    [SerializeField] PolygonCollider2D fallenTreePolygonCollider;
    private ParticleSystem woodchips;

    private void Start()
    {
        hits = 0;
        treeAnimator = GetComponentInChildren<Animator>();
        treePolygonCollider = GetComponent<PolygonCollider2D>();
        woodchips = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<PlayerMovement>();
            if (player.playerIsRolling)
            {
                if (hits < 3)
                {
                    TriggerParticles();
                }
                hits++;
                if (hits == 3)
                {
                    treeAnimator.SetBool("isFalling", true);
                    ChangeCollider();
                }
            }
        }
    }
    private void ChangeCollider()
    {
        treePolygonCollider.enabled = false;
        fallenTreePolygonCollider.enabled = true;
    }

    private void TriggerParticles()
    {
        woodchips.Emit(25);
    }
}
