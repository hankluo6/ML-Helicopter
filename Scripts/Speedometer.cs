/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour {

    public PhantomDial dataLog;
    private Transform needleTranform;
    private Transform speedLabelTemplateTransform;

    private float maxSpeed;
    private float minSpeed;
    private float maxAngle;
    private float minAngle;

    private void Awake() {
        needleTranform = transform.Find("needle");
        speedLabelTemplateTransform = transform.Find("speedLabelTemplate");
        speedLabelTemplateTransform.gameObject.SetActive(false);
    }

    private void Start()
    {
        maxSpeed = dataLog.MaximumValue;
        minSpeed = 0;
        maxAngle = dataLog.MaximumAngleDegrees;
        minAngle = 0;
        CreateSpeedLabels();
    }

    private void Update()
    {
        float currentValue = dataLog.currentValue;
        //Calculate Difference
        float valueDelta = (currentValue - minSpeed) / (maxSpeed - minSpeed);
        float angleDeltaDegrees = ((maxAngle - minAngle) * valueDelta);
        //Move needle to appropriate Point
        needleTranform.localRotation = Quaternion.Euler(0, 0, 90);
        needleTranform.Rotate(new Vector3(0, 0, -1), angleDeltaDegrees);
    }


    private void CreateSpeedLabels() {
        int labelAmount = 10;
        float totalAngleSize = maxAngle - minAngle;

        for (int i = 0; i <= labelAmount; i++) {
            Transform speedLabelTransform = Instantiate(speedLabelTemplateTransform, transform);
            float labelSpeedNormalized = (float)i / labelAmount;
            float speedLabelAngle = 90 - labelSpeedNormalized * totalAngleSize;
            speedLabelTransform.eulerAngles = new Vector3(0, 0, speedLabelAngle);
            speedLabelTransform.Find("speedText").GetComponent<Text>().text = Mathf.RoundToInt(labelSpeedNormalized * maxSpeed).ToString();
            speedLabelTransform.Find("speedText").eulerAngles = Vector3.zero;
            speedLabelTransform.gameObject.SetActive(true);
        }

        needleTranform.SetAsLastSibling();
    }
}
