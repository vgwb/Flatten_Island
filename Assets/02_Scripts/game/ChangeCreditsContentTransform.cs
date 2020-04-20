using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCreditsContentTransform : MonoBehaviour
{

    public RectTransform ContentStartPosition;
    public RectTransform ContentEndPosition;
    float StartTime;
    float TotalDistanceToDestination;

    void Start()
    {
        StartTime = Time.time;
        TotalDistanceToDestination = Vector3.Distance(ContentStartPosition.position, ContentEndPosition.position);
    }
    void Update()
    {
        float currentDuration = Time.time - StartTime;
        float journeyFraction = currentDuration / TotalDistanceToDestination;
        transform.position = Vector3.Lerp(ContentStartPosition.position, ContentEndPosition.position, journeyFraction);
    }


}
