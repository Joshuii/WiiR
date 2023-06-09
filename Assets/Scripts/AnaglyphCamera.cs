using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnaglyphCamera : MonoBehaviour
{
    public Camera Camera;

    [Header("Screen")]
    [SerializeField]
    private Transform BottomLeft;
    [SerializeField]
    private Transform BottomRight;
    [SerializeField]
    private Transform TopLeft;
    [SerializeField]
    private Transform TopRight;

    private Material anaglyphMaterial;

    private bool anaglyphEnabled = true;

    private void Awake()
    {
        anaglyphMaterial = new Material(Resources.Load<Shader>("Shaders/Anaglyph"));
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            anaglyphEnabled = !anaglyphEnabled;
        }
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (anaglyphEnabled)
        {
            RenderTexture leftTexture = RenderTexture.GetTemporary(src.descriptor);
            RenderTexture rightTexture = RenderTexture.GetTemporary(src.descriptor);

            // Left eye
            CalculateProjectionMatrix(Vector3.left * 0.1f);
            Camera.targetTexture = leftTexture;
            Camera.Render();
            anaglyphMaterial.SetTexture("_LeftTex", leftTexture);

            // Right eye
            CalculateProjectionMatrix(Vector3.right * 0.1f);
            Camera.targetTexture = rightTexture;
            Camera.Render();
            anaglyphMaterial.SetTexture("_RightTex", rightTexture);

            Graphics.Blit(src, dst, anaglyphMaterial);
            RenderTexture.ReleaseTemporary(leftTexture);
            RenderTexture.ReleaseTemporary(rightTexture);
        }
        else
        {
            RenderTexture texture = RenderTexture.GetTemporary(src.descriptor);
            CalculateProjectionMatrix(Vector3.zero);
            Camera.targetTexture = texture;
            Camera.Render();
            Graphics.Blit(texture, dst);
            RenderTexture.ReleaseTemporary(texture);
        }
    }

    public void CalculateProjectionMatrix(Vector3 offset)
    {
        float nearDistance = Camera.nearClipPlane;
        float farDistance = Camera.farClipPlane;

        Vector3 eyePosition = Camera.transform.position + offset;
        // Eye to bottom left
        Vector3 va = BottomLeft.position - eyePosition;
        // Eye to bottom right
        Vector3 vb = BottomRight.position - eyePosition;
        // Eye to top left
        Vector3 vc = TopLeft.position - eyePosition;

        // Right screen vector
        Vector3 vr = (BottomRight.position - BottomLeft.position).normalized;
        // Up screen vector
        Vector3 vu = (TopLeft.position - BottomLeft.position).normalized;
        // Normal screen vector
        Vector3 vn = -Vector3.Cross(vr, vu).normalized;

        // Distance from the screen to eye
        float eyeDistance = eyePosition.magnitude;

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

        Camera.worldToCameraMatrix =
            transformation *
            Matrix4x4.Rotate(Quaternion.Inverse(Camera.transform.rotation)) *
            Matrix4x4.Translate(-eyePosition);
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
}
