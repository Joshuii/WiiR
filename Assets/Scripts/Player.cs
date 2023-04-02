using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using WiimoteApi.Util;

public class Player : MonoBehaviour
{
    [SerializeField]
    private WiimoteConnectionManager connectionManager;
    [SerializeField]
    private AnaglyphCamera anaglyphCamera;

    [SerializeField]
    private float MovementSpeed;

    [Header("Head Tracking")]
    [SerializeField]
    private float HeadMovementSpeed = 1f;
    [SerializeField]
    private float ScreenHeightMM = 195f;

    [Header("Gun")]
    [SerializeField]
    private RectTransform crosshair;
    private Vector3 screenPoint;

    public InputAction Move;

    private Vector3 HeadPosition = Vector3.up;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 input = Move.ReadValue<Vector2>().normalized;
        //Vector2 headVelocity = input * MovementSpeed * Time.deltaTime;
        //HeadPosition += new Vector3(headVelocity.x, 0, headVelocity.y);

        UpdateGunPosition();
        UpdateHeadPosition();
        transform.position = new Vector3(-HeadPosition.x, HeadPosition.y, -HeadPosition.z);
    }

    private void UpdateHeadPosition()
    {
        if (connectionManager.HeadWiimote == null)
        {
            return;
        }

        List<Vector2> points = new();
        ReadOnlyMatrix<int> ir = connectionManager.HeadWiimote.Ir.ir;

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
            //float radiansPerPixel = (Mathf.Deg2Rad * 33f) / 1024f;
            float radiansPerPixel = (Mathf.PI / 8f) / 1024f;
            float angle = radiansPerPixel * (pointDistance / 2f);
            //float sensorBarWidth = 200f;
            float sensorBarWidth = 8.5f * 25.4f;
            HeadPosition.z = HeadMovementSpeed * ((sensorBarWidth / 2f) / Mathf.Tan(angle)) / ScreenHeightMM;
            HeadPosition.x = HeadMovementSpeed * Mathf.Sin(radiansPerPixel * (centerPoint.x - 512f)) * HeadPosition.z;
            float relativeVerticalAngle = (centerPoint.y - 384f) * radiansPerPixel;
            HeadPosition.y = -0.5f + (HeadMovementSpeed * Mathf.Sin(relativeVerticalAngle + (Mathf.Deg2Rad * 10f)) * HeadPosition.z);
            Debug.Log($"X: {HeadPosition.x}, Y: {HeadPosition.y}, Z: {HeadPosition.z}");
        }
    }

    private void UpdateGunPosition()
    {
        if (connectionManager.HandWiimote == null)
        {
            return;
        }

        float[] coordinates = connectionManager.HandWiimote.Ir.GetPointingPosition();
        Vector2 pointerPosition = new(coordinates[0], coordinates[1]);

        if (pointerPosition == Vector2.zero)
        {
            return;
        }

        anaglyphCamera.CalculateProjectionMatrix(Vector3.zero);
        screenPoint = Vector3.Lerp(screenPoint, anaglyphCamera.Camera.ViewportToScreenPoint(pointerPosition), 0.5f);
        Ray ray = anaglyphCamera.Camera.ScreenPointToRay(screenPoint);
        crosshair.position = screenPoint;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log(hit.transform.gameObject.name);
        }
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
