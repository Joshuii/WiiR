using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<Segment> liveSegments;

    public bool IsStarted { get; private set; } = false;

    public float Speed { get; private set; } = 0;

    public float Distance { get; private set; } = 0;

    [SerializeField]
    float InitialSpeed;

    [SerializeField]
    float SpeedIncreasePerSecond;

    [SerializeField]
    float SegmentLoadDistance;

    [SerializeField]
    float LengthPerSegment;

    [SerializeField]
    float GapBetweenSegments;

    [SerializeField]
    float MaxSpeed;

    [SerializeField]
    GameObject[] Segments;

    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsStarted)
        {
            Distance += Speed * Time.deltaTime;
            Speed += SpeedIncreasePerSecond * Time.deltaTime;
            Speed = Mathf.Min(Speed, MaxSpeed);
            GenerateMoreSegments();
            UpdateSegmentVelocitys();
            DestroyOldSegments();
        }
    }

    public void StartGame()
    {
        if (IsStarted)
        {
            Debug.LogError("Tried to start game when game already started");
            return;
        }

        Speed = InitialSpeed;
        Distance = 0;
        liveSegments = new List<Segment>();
        IsStarted = true;
    }

    public void StopGame()
    {
        if (!IsStarted)
        {
            Debug.LogError("Tried to stop game when game not started");
            return;
        }

        foreach (Segment segment in liveSegments)
        {
            Destroy(segment.GameObject);
        }

        IsStarted = false;
    }

    private void GenerateMoreSegments()
    {
        while (liveSegments.Count == 0 || liveSegments[liveSegments.Count-1].Distance < Distance + SegmentLoadDistance)
        {
            float newSegDist = liveSegments.Count == 0 ? GapBetweenSegments : liveSegments[liveSegments.Count - 1].Distance + GapBetweenSegments + LengthPerSegment;
            GameObject newSegment = Object.Instantiate(Segments[Random.Range(0, Segments.Length)], new Vector3(0, 0, newSegDist - Distance), Quaternion.identity);
            liveSegments.Add(new Segment(newSegment, newSegDist));
        }
    }

    private void UpdateSegmentVelocitys()
    {
        foreach (Segment segment in liveSegments)
        {
            segment.UpdatePosition(Distance);
        }
    }

    private void DestroyOldSegments()
    {
        while (liveSegments.Count > 0 && liveSegments[0].Distance < Distance - LengthPerSegment - GapBetweenSegments)
        {
            Destroy(liveSegments[0].GameObject);
            liveSegments.RemoveAt(0);
        }
    }

    private class Segment
    {
        public Segment(GameObject gameObject, float distance)
        {
            GameObject = gameObject;
            Distance = distance;
        }

        public GameObject GameObject { get; }
        public float Distance { get; }

        public void UpdatePosition(float distance)
        {
            GameObject.transform.position = new Vector3(0, 0, Distance - distance);
        }
    }
}
