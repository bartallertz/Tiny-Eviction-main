using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceSafeArea : MonoBehaviour
{

    private Rect lastSafeArea;
    private RectTransform parentRectTransform;


    // Start is called before the first frame update
    void Start()
    {
        parentRectTransform = this.GetComponentInParent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lastSafeArea != Screen.safeArea ) {
            ApplyDeviceSafeArea();
        }
    }

    private void ApplyDeviceSafeArea() {
        Rect safeAreaRect = Screen.safeArea;

        float scaleRatio = parentRectTransform.rect.width / Screen.width;

        var left = safeAreaRect.xMin * scaleRatio;
        var right = -( Screen.width - safeAreaRect.xMax ) * scaleRatio;
        var top = -safeAreaRect.yMin * scaleRatio;
        var bottom = ( Screen.height - safeAreaRect.yMax ) * scaleRatio;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.offsetMin = new Vector2( left, bottom );
        rectTransform.offsetMax = new Vector2( right, top );

        lastSafeArea = Screen.safeArea;
    }
}
