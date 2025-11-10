using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    [SerializeField]
    private string key;
    public Image unlockImage;
    private bool unlocked;

    private void Start()
    {
        UpdateLevelImage();
    }

    private void UpdateLevelImage()
    {
        if ((PlayerPrefs.HasKey(key)) || (PlayerPrefs.HasKey("hasAllLevelsUnlocked")))
        {
            unlocked = true;
            unlockImage.gameObject.SetActive(false);
        }
        else
        {
            unlocked = false;
            unlockImage.gameObject.SetActive(true);
        }
    }


    public void LevelSelection(string levelName)
    {
        if (unlocked == true)
        {
            SceneManager.LoadScene(levelName);
        }
    }

}
