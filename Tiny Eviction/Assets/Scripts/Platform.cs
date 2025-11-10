using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Platform : MonoBehaviour
{

    [SerializeField] private Transform PlatformPosition;
    private Collision2D coll;
    [SerializeField] private GameObject player;
    [SerializeField] bool isOnPlatform = false;

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            isOnPlatform = true;
            player.transform.position = new Vector3(PlatformPosition.position.x, player.transform.position.y, player.transform.position.z);
        }
    }
    private void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            isOnPlatform = true;
            player.transform.position = new Vector3(PlatformPosition.position.x, player.transform.position.y, player.transform.position.z);

        }
    }
    private void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            isOnPlatform = true;
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        }
    }
}
