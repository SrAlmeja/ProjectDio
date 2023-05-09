using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRecorder : MonoBehaviour
{
    [SerializeField] private float recordInterval = 1.0f;
    [SerializeField] private PlaybackData playbackData;

    private float lastRecordTime;

    private void Start()
    {
        lastRecordTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - lastRecordTime >= recordInterval)
        {
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            float time = Time.time;

            playbackData.AddRecord(position, rotation, time);

            lastRecordTime = time;
        }
    }
}
