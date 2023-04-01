using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [SerializeField]
    private Camera Camera;

    [SerializeField]
    private float MovementSpeed;

    [SerializeField]
    private float ScreenHeight;

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
