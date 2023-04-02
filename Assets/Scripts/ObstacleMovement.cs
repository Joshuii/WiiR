using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    [SerializeField]
    Vector2 FromPosition;

    [SerializeField]
    Vector2 ToPosition;

    [SerializeField]
    float CycleSeconds;

    [SerializeField]
    float CycleDelaySeconds;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPos = Vector3.Lerp(FromPosition, ToPosition, (Mathf.Sin(((Time.timeSinceLevelLoad - CycleDelaySeconds) / CycleSeconds) * Mathf.PI) / 2) + 0.5f);
        transform.localPosition = new Vector3(newPos.x, newPos.y, transform.localPosition.z);
    }
}
