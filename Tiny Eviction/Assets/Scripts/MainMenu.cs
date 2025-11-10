using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{   
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("Lv1"))
        {
            PlayerPrefs.SetInt("Lv1", 1);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }
    public void OpenInfo(){
        Application.OpenURL("https://linktr.ee/blindbananastudio");
    }
}
