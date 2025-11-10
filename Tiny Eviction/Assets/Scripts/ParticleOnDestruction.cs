using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleOnDestruction : MonoBehaviour
{
    [SerializeField] private ParticleSystem CollisionParticleSystem;
    [SerializeField] private SpriteRenderer sr;
    public bool once = true;

    [SerializeField] private AudioSource source;


    private void OnTriggerEnter2D(Collider2D coll){
        if(coll.gameObject.CompareTag("Player") && once == true){

            var em = CollisionParticleSystem.emission;
            var dur = CollisionParticleSystem.duration;

          
            source.Play();
            em.enabled = true;;
            CollisionParticleSystem.Play();

            once = false;
            Destroy(sr);
            Invoke(nameof(DestroyObj), dur);
        }
    }

    void DestroyObj(){
        Destroy(gameObject);
    }
}
