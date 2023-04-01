using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using WiimoteApi.Util;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Camera Camera;

    [SerializeField]
    private float MovementSpeed;

    [SerializeField]
    private float HeadMovementSpeed = 1f;

    [SerializeField]
    private float ScreenHeightMM = 195f;

    [SerializeField]
    private WiimoteConnectionManager ConnectionManager;

    public InputAction Move;

    private Vector3 HeadPosition = Vector3.up;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = Move.ReadValue<Vector2>().normalized;
        Vector2 headVelocity = input * MovementSpeed * Time.deltaTime;
        HeadPosition += new Vector3(headVelocity.x, 0, headVelocity.y);

        transform.position = HeadPosition;

        //camera.projectionMatrix;

        List<Vector2> points = new();
        ReadOnlyMatrix<int> ir = ConnectionManager.HeadWiimote.Ir.ir;

        // Read LED points from Wiimote IR camera
        for (int i = 0; i < 4; i++)
        {
            if (ir[i, 0] >= 0)
            {
                points.Add(new Vector2(ir[i, 0], ir[i, 1]));
            }
        }

        if (points.Count >= 2)
        {
            // Calculate head-mounted sensor bar position
            Vector2 delta = points[0] - points[1];
            Vector2 centerPoint = (points[0] + points[1]) / 2f;
            float pointDistance = Mathf.Sqrt(delta.sqrMagnitude);
            float radiansPerPixel = (Mathf.PI / 4f) / 1024f;
            float angle = radiansPerPixel * (pointDistance / 2f);
            float sensorBarWidth = 200f;
            float headZ = HeadMovementSpeed * ((sensorBarWidth / 2f) / Mathf.Tan(angle)) / ScreenHeightMM;
            float headX = HeadMovementSpeed * Mathf.Sin(radiansPerPixel * (centerPoint.x - 512f)) * headZ;
            float relativeVerticalAngle = (centerPoint.y - 384f) * radiansPerPixel;
            float headY = -0.5f + (HeadMovementSpeed * Mathf.Sin(relativeVerticalAngle + 0f) * headZ);
            Debug.Log($"X: {headX}, Y: {headY}, Z: {headZ}");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(0, 0, 0), new Vector3(1, 1, 1));
    }

    private void OnEnable()
    {
        Move.Enable();
    }

    private void OnDisable()
    {
        Move.Disable();
    }
}
