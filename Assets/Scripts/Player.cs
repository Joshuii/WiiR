using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    [SerializeField] private Camera PlayerCamera;

    public InputAction Move;

    [SerializeField] private float MovementSpeed;

    private Vector3 HeadPosition = Vector3.up;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        Vector2 input = Move.ReadValue<Vector2>().normalized;
        Vector2 headVelocity = input * MovementSpeed * Time.deltaTime;
        HeadPosition += new Vector3(headVelocity.x, 0, headVelocity.y);

        transform.position = HeadPosition;
    }

    private void OnEnable() {
        Move.Enable();
    }

    private void OnDisable() {
        Move.Disable();
    }
}
