using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int CurrentIndex = 0;

    [SerializeField] private float speed = 2f;

    // Update is called once per frame
    private void Update()
    {
        if(Vector2.Distance(waypoints[CurrentIndex].transform.position,transform.position) <= .1f ){
            CurrentIndex++;
            if(CurrentIndex >= waypoints.Length){
                CurrentIndex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[CurrentIndex].transform.position, Time.deltaTime * speed);
    }
}
