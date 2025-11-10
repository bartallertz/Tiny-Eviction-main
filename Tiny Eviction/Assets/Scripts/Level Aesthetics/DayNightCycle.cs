using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    [Header("Required Objects")]
    public LevelTimer levelTimer;
    public RectTransform sunAndMoonContainer;
    public Camera mainCamera;

    [Header("Light Sources")]
    [SerializeField] public GameObject sun;
    private Light2D sunLightComponent;
    [SerializeField] public GameObject virtualSun;
    private Light2D virtualSunLightComponent;
    [SerializeField] public GameObject moon;
    private Light2D moonLightComponent;
    [SerializeField] public GameObject virtualMoon;
    private Light2D virtualMoonLightComponent;

    [Header("Light Variables")]
    [SerializeField] public float sunMinIntensity;
    [SerializeField] public float sunMaxIntensity;
    [SerializeField] public float virtualSunMinIntensity;
    [SerializeField] public float virtualSunMaxIntensity;
    [SerializeField] public float moonMinIntensity;
    [SerializeField] public float moonMaxIntensity;
    [SerializeField] public float virtualMoonMinIntensity;
    [SerializeField] public float virtualMoonMaxIntensity;

    private float sunAndMoonRotationPerSecond;
    private float sunAndMoonTotalRotation = 270f;
    private float sunAndMoonRotation;

    private float sunRiseStartTime;
    private float sunMidRiseTime;
    private float sunRiseEndTime;
    private float sunSetMidTime;
    private float sunSetEndTime;
    private float moonMidRiseTime;
    private float dayNightCycleTotalTime;

    private float sunIntensityPerSecond;
    private float virtualSunIntensityPerSecond;
    private float moonIntensityPerSecond;
    private float virtualMoonIntensityPerSecond;


    void Awake(){
        // Get sun and virtualSun
        sunLightComponent = sun.GetComponent<Light2D>();
        sunLightComponent.intensity = sunMinIntensity;
        virtualSunLightComponent = virtualSun.GetComponent<Light2D>();
        virtualSunLightComponent.intensity = virtualSunMinIntensity;
        sunLightComponent.gameObject.SetActive(true);
        // Get moon and virtualMoon, disable right away
        moonLightComponent = moon.GetComponent<Light2D>();
        moonLightComponent.intensity = moonMinIntensity;
        virtualMoonLightComponent = virtualMoon.GetComponentInChildren<Light2D>();
        virtualMoonLightComponent.intensity = virtualMoonMaxIntensity;
        moonLightComponent.gameObject.SetActive(false);

        // Get time parts, rotations and intensities
        calculateTimeRotationAndIntensity();

        // Get our camera size (orthographicSize = half of width)
        float totalCameraHeight = mainCamera.orthographicSize * 2;
        float totalCameraWidth = totalCameraHeight * mainCamera.aspect;

        // Set sun/moon container to be camera width x camera width and place it correctly
        sunAndMoonContainer.sizeDelta = new Vector2(totalCameraWidth, totalCameraWidth);
        float yPositionOfContainer = sunAndMoonContainer.rect.size.y / 4f;
        sunAndMoonContainer.localPosition = new Vector3(sunAndMoonContainer.position.x, -yPositionOfContainer, 0);
    }
    void Update()
    {
        if (!levelTimer.isTimerPaused) {
            // Light and rotate sun and moon during timer
            lightSunAndMoon();
            rotateSunAndMoon();
        }
    }

    void calculateTimeRotationAndIntensity(){
        // Calc times of each day night cycle case
        float cycleStep = levelTimer.startingTime / 6;
        sunRiseStartTime = 0f;
        sunMidRiseTime = cycleStep;
        sunRiseEndTime = cycleStep * 2;
        sunSetMidTime = cycleStep * 3;
        sunSetEndTime = cycleStep * 4;
        moonMidRiseTime = cycleStep * 5;
        dayNightCycleTotalTime = levelTimer.startingTime;


        // Calc rotation of the container (including sun and moon)
        sunAndMoonRotationPerSecond = sunAndMoonTotalRotation / levelTimer.startingTime;

        // Calc intensity of sun and virtual sun
        float sunAllowedIntensityChange = sunMaxIntensity - sunMinIntensity;
        sunIntensityPerSecond = sunAllowedIntensityChange / cycleStep;
        float virtualSunAllowedIntensityChange = virtualSunMaxIntensity - virtualSunMinIntensity;
        virtualSunIntensityPerSecond = virtualSunAllowedIntensityChange / (cycleStep * 2);

        // Calc intensity of moon and virtual moon
        float moonAllowedIntensityChange = moonMaxIntensity - moonMinIntensity;
        moonIntensityPerSecond = moonAllowedIntensityChange / cycleStep;
        float virtualMoonAllowedIntensityChange = virtualMoonMaxIntensity - virtualMoonMinIntensity;
        virtualMoonIntensityPerSecond = virtualMoonAllowedIntensityChange / (cycleStep * 2);
    }
    
    void rotateSunAndMoon()
    {
        sunAndMoonRotation = -(sunAndMoonRotationPerSecond * (levelTimer.startingTime - levelTimer.currentTime));

        // Slowly rotate our sun/moon container as the timer ticks down
        sunAndMoonContainer.transform.rotation
            = UnityEngine.Quaternion.RotateTowards(
                sunAndMoonContainer.transform.rotation,
                UnityEngine.Quaternion.Euler(0, 0, sunAndMoonRotation),
                sunAndMoonRotationPerSecond / 20);
    }

    void lightSunAndMoon()
    {

        float totalTimePassed = levelTimer.startingTime - levelTimer.currentTime;

        // Switch between lightening and darkening either the moon or the sun.
        switch (Mathf.Abs(totalTimePassed))
        {
            // SUN MID RISE, brighten sun fully, virtual sun halfway
            case float total when (total > sunRiseStartTime && total <= sunMidRiseTime):
                sunLightComponent.intensity = Mathf.Lerp(sunLightComponent.intensity, (sunMinIntensity + (sunIntensityPerSecond * (totalTimePassed - sunRiseStartTime))) , sunIntensityPerSecond);
                virtualSunLightComponent.intensity = Mathf.Lerp(virtualSunLightComponent.intensity, (virtualSunMinIntensity + (virtualSunIntensityPerSecond * (totalTimePassed - sunRiseStartTime))) , virtualSunIntensityPerSecond);
                break;

            // SUN HIGH, brighten virtual sun fully
            case float total when (total > sunMidRiseTime && total <= sunRiseEndTime):
                sunLightComponent.intensity = sunMaxIntensity;
                virtualSunLightComponent.intensity = Mathf.Lerp(virtualSunLightComponent.intensity, (virtualSunMinIntensity + (virtualSunIntensityPerSecond * (totalTimePassed - sunRiseStartTime))) , virtualSunIntensityPerSecond);
                break;

            // SUN MID SET, dim virtual sun halfway
            case float total when (total > sunRiseEndTime && total <= sunSetMidTime):
                virtualSunLightComponent.intensity = Mathf.Lerp(virtualSunLightComponent.intensity, (virtualSunMaxIntensity - (virtualSunIntensityPerSecond * (totalTimePassed - sunRiseEndTime))) , virtualSunIntensityPerSecond);
                break;

            // SUN SET, dim sun fully, virtual sun fully
            case float total when (total > sunSetMidTime && total <= sunSetEndTime):
                sunLightComponent.intensity = Mathf.Lerp(sunLightComponent.intensity, (sunMaxIntensity - (sunIntensityPerSecond * (totalTimePassed - sunSetMidTime))) , sunIntensityPerSecond);
                virtualSunLightComponent.intensity = Mathf.Lerp(virtualSunLightComponent.intensity, (virtualSunMaxIntensity - (virtualSunIntensityPerSecond * (totalTimePassed - sunRiseEndTime))) , virtualSunIntensityPerSecond );
                break;

            // MOON RISE HALFWAY, disable sun, enable moon, brighten moon fully, virtual moon halfway
            case float total when (total > sunSetEndTime && total <= moonMidRiseTime):
                if(!moonLightComponent.gameObject.activeSelf){
                    moonLightComponent.gameObject.SetActive(true);
                    virtualSunLightComponent.gameObject.SetActive(false);
                }
                if(sunLightComponent.intensity < 0){
                    sunLightComponent.intensity = 0f;
                    sunLightComponent.gameObject.SetActive(false);
                } else {
                    sunLightComponent.intensity = Mathf.Lerp(sunLightComponent.intensity, (sunMaxIntensity - (sunIntensityPerSecond * (totalTimePassed - sunSetMidTime))) , sunIntensityPerSecond);
                }
                moonLightComponent.intensity = Mathf.Lerp(moonLightComponent.intensity, (moonIntensityPerSecond + (moonIntensityPerSecond * (totalTimePassed - sunSetEndTime))) , moonIntensityPerSecond);
                virtualMoonLightComponent.intensity = Mathf.Lerp(virtualMoonLightComponent.intensity, (virtualMoonMaxIntensity - (virtualMoonIntensityPerSecond * (totalTimePassed - sunSetEndTime))) , virtualMoonIntensityPerSecond);
                break;

            // MOON HIGH, brighten virtual moon fully
            case float total when (total > moonMidRiseTime && total <= dayNightCycleTotalTime):
                moonLightComponent.intensity = moonMaxIntensity;
                virtualMoonLightComponent.intensity = Mathf.Lerp(virtualMoonLightComponent.intensity, (virtualMoonMaxIntensity - (virtualMoonIntensityPerSecond * (totalTimePassed - sunSetEndTime))) , virtualMoonIntensityPerSecond);
                break;

            default: break;
        }
    }
}
