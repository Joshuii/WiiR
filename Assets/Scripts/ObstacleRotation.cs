using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRotation : MonoBehaviour
{
    [SerializeField]
    float RotationTimeSeconds;

    [SerializeField]
    float CycleDelaySeconds;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(0, 0, ((Time.timeSinceLevelLoad - CycleDelaySeconds) / RotationTimeSeconds) * 360);
    }
}
