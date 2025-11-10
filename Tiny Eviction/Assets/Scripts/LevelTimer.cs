using System.Numerics;
using System;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LevelTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] public float currentTime;
    [SerializeField] public float startingTime = 120f;
    [SerializeField] public bool isTimerPaused;

    [Header("Cycle Settings")]
    [SerializeField] public float startingRotation = 0f;
    [SerializeField] public float targetRotation = 180f;

    [Header("Timer Components")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject timerCycle;

    [Header("Video")]
    [SerializeField] private GameObject GameOver;
    [SerializeField] private VideoPlayer GameOverVideo;
    [SerializeField] private GameObject GameOverPopUp;

    [Header("Audio")]
    [SerializeField] private AudioSource DayTime;

    [SerializeField] private AudioSource NightTime;

    [SerializeField] private bool HalfDay;


    private float totalRotation;
    private float completedRotation;
    private float rotationPerSecond;

    private bool videoIsPlaying = false;

    private void Start()
    {
        levelText.text = SceneManager.GetActiveScene().name;

        currentTime = startingTime;
        totalRotation = targetRotation - startingRotation;
        rotationPerSecond = totalRotation / startingTime;

        DayTime.Play();
    }

    private void CheckMiddle()
    {

        if (DayTime.isPlaying)
        {

            if (currentTime <= startingTime / 2)
            {
                HalfDay = true;
                if (HalfDay)
                {
                    DayTime.Stop();
                    NightTime.Play();
                }
            }
        }
    }

    private void Update()
    {
        // if currenttime = half of start time
        // play 2nd music
        // else play 1st music
        CheckMiddle();
        if (!isTimerPaused)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
            }
            else
            {
                currentTime = 0;
                if (!videoIsPlaying){
                    GameOverSequence();
                }
            }
            DisplayTime(currentTime);
            UpdateCycle();
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }

    private void UpdateCycle()
    {
        completedRotation = (rotationPerSecond * (startingTime - currentTime));

        timerCycle.transform.rotation
            = UnityEngine.Quaternion.RotateTowards(
                timerCycle.transform.rotation,
                UnityEngine.Quaternion.Euler(0, 0, startingRotation + completedRotation),
                rotationPerSecond / 20);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GameOverSequence()
    {
        if (GameOver != null)
        {
            videoIsPlaying = true;
            GameOver.SetActive(true);
            GameOverVideo.frame = 0;
            GameOverVideo.Play();
            GameOverVideo.loopPointReached += EndReached;
        }

    }

    void EndReached(VideoPlayer gameOverVideo){
        GameOverPopUpToActive();
    }

    public void GameOverPopUpToActive()
    {
        GameOverPopUp.SetActive(true);
        if (GameOver != null)
        {
            GameOver.SetActive(false);
        }

    }
}
