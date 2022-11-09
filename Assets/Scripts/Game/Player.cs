using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public float VerticalSpeedFactor { get; set; } = 0.5f;
    public bool WasPlayerHit { get; private set; }

    private float gravity = 9.8f;
    private float verticalSpeed;
    private CharacterController characterController;
    private int verticalDirectionFactor;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))
            verticalDirectionFactor = 1;
        else
            verticalDirectionFactor = -1;

        verticalSpeed = verticalDirectionFactor * gravity * VerticalSpeedFactor * Time.deltaTime;
        movement.y = verticalSpeed;
        characterController.Move(movement);
    }
    
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        WasPlayerHit = true;
    }
}