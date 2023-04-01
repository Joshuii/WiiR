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

    [Header("Screen")]
    [SerializeField]
    private Transform BottomLeft;
    [SerializeField]
    private Transform BottomRight;
    [SerializeField]
    private Transform TopLeft;
    [SerializeField]
    private Transform TopRight;

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

        UpdateHeadPosition();
        transform.position = new Vector3(-HeadPosition.x, HeadPosition.y, -HeadPosition.z);
    }

    private void UpdateHeadPosition()
    {
        if (ConnectionManager.HeadWiimote == null)
        {
            return;
        }

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

    private void LateUpdate()
    {
        //CalculateProjectionMatrix(Camera.nearClipPlane, Camera.farClipPlane);
    }

    private void CalculateProjectionMatrix(float nearDistance, float farDistance)
    {
        // Eye to bottom left
        Vector3 va = BottomLeft.position - Camera.transform.position;
        // Eye to bottom right
        Vector3 vb = BottomRight.position - Camera.transform.position;
        // Eye to top left
        Vector3 vc = TopLeft.position - Camera.transform.position;

        // Right screen vector
        Vector3 vr = (BottomRight.position - BottomLeft.position).normalized;
        // Up screen vector
        Vector3 vu = (TopLeft.position - BottomLeft.position).normalized;
        // Normal screen vector
        Vector3 vn = -Vector3.Cross(vr, vu).normalized;

        // Distance from the screen to eye
        float eyeDistance = Camera.transform.position.magnitude;

        Debug.Log(eyeDistance);

        float left = Vector3.Dot(vr, va) * (nearDistance / eyeDistance);
        float right = Vector3.Dot(vr, vb) * (nearDistance / eyeDistance);
        float bottom = Vector3.Dot(vu, va) * (nearDistance / eyeDistance);
        float top = Vector3.Dot(vu, vc) * (nearDistance / eyeDistance);

        Matrix4x4 projection = Matrix4x4.Frustum(left, right, bottom, top, nearDistance, farDistance);

        Matrix4x4 transformation = new();
        transformation[0, 0] = vr.x;
        transformation[0, 1] = vr.y;
        transformation[0, 2] = vr.z;
        transformation[0, 3] = 0;

        transformation[1, 0] = vu.x;
        transformation[1, 1] = vu.y;
        transformation[1, 2] = vu.z;
        transformation[1, 3] = 0;

        transformation[2, 0] = vn.x;
        transformation[2, 1] = vn.y;
        transformation[2, 2] = vn.z;
        transformation[2, 3] = 0;

        transformation[3, 0] = 0;
        transformation[3, 1] = 0;
        transformation[3, 2] = 0;
        transformation[3, 3] = 1;

        Matrix4x4 camera = new();
        camera[0, 0] = 1;
        camera[0, 1] = 0;
        camera[0, 2] = 0;
        camera[0, 3] = 0;

        camera[1, 0] = 0;
        camera[1, 1] = 1;
        camera[1, 2] = 0;
        camera[1, 3] = 0;

        camera[2, 0] = 0;
        camera[2, 1] = 0;
        camera[2, 2] = 1;
        camera[2, 3] = 0;

        camera[3, 0] = -Camera.transform.position.x;
        camera[3, 1] = -Camera.transform.position.y;
        camera[3, 2] = -Camera.transform.position.z;
        camera[3, 3] = 1;

        Camera.worldToCameraMatrix =
            transformation *
            Matrix4x4.Rotate(Quaternion.Inverse(Camera.transform.rotation)) *
            Matrix4x4.Translate(-Camera.transform.position);
        Camera.projectionMatrix = projection;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(BottomLeft.position, BottomRight.position);
        Gizmos.DrawLine(BottomRight.position, TopRight.position);
        Gizmos.DrawLine(TopRight.position, TopLeft.position);
        Gizmos.DrawLine(TopLeft.position, BottomLeft.position);
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
