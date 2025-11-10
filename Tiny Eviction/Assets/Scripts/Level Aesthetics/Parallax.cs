using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    
    public float parallaxEffect; // Intensity of parallax effect
    public bool canMoveVertically; // Whether a layer can also move horizontally (IE sky can, clouds and trees cant)
    public GameObject mainCamera; // The main scene camera

    private float length;
    private float height;
    private float startPosX;
    private float startPosY;

    // Start is called before the first frame update
    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;

        getAppropriateBounds();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontalRelativetyToCam = (mainCamera.transform.position.x * (1 - parallaxEffect));
        float horizontalDistance = (mainCamera.transform.position.x * parallaxEffect);

        if (canMoveVertically) {
            float cameraPosY = (mainCamera.transform.position.y);
            transform.position = new Vector3(startPosX + horizontalDistance, cameraPosY, transform.position.z);
        }
        else {
            transform.position = new Vector3(startPosX + horizontalDistance, transform.position.y, transform.position.z);
        }

        if (horizontalRelativetyToCam > startPosX + length) {
            startPosX += length;
        }
        else if (horizontalRelativetyToCam < startPosX - length) {
            startPosX -= length;
        }
    }

    // Set bounds of element to be parallaxed
    void getAppropriateBounds(){
        // Bounds of object with a SpriteRenderer as direct component (Sprite size)
        if (GetComponent<SpriteRenderer>() != null){
            length = GetComponent<SpriteRenderer>().bounds.size.x;
            height = GetComponent<SpriteRenderer>().bounds.size.y;
        }
        // Bounds of object with multiple SriteRenderers (composite RectTransform size)
        else if (GetComponentsInChildren<SpriteRenderer>() != null) {
            GameObject containerObject = transform.GetChild(0).gameObject;
            RectTransform containerRect = containerObject.GetComponent<RectTransform>();
            length = containerRect.rect.size.x;
            height = containerRect.rect.size.y;
        }
    }
}
