using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCompletedScreen : MonoBehaviour
{
    [SerializeField] GameObject continueContainer;

    private bool canBeClosed = false;

    void Start()
    {
        prepareScreen();
    }

    void Update()
    {
        if (canBeClosed && continueContainer.activeSelf){
            listenForSkip();
        }
    }

    private void prepareScreen()
    {
        continueContainer.SetActive(false); // Just in case the button is active for some reason
        Invoke("showContinueButton", 3);
    }

    private void showContinueButton(){
        canBeClosed = true;
        continueContainer.SetActive(true);
    }

    public void listenForSkip(){
        if (Input.touchCount != 0){
            if (Input.GetTouch(0).phase == TouchPhase.Began){
                SceneManager.LoadScene("LevelSelect");
            }
        }
    }
}
