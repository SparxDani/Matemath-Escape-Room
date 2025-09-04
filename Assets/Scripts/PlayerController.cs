using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 2f;
    public Transform cameraTransform;

    private Rigidbody rb;
    float verticalRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
    // Rotación horizontal (Player)
    float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
    transform.Rotate(Vector3.up * mouseX);

    // Rotación vertical (Camera)
    float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
    verticalRotation -= mouseY;
    verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f);
    cameraTransform.localEulerAngles = new Vector3(verticalRotation, 0f, 0f);
    // La posición de la cámara nunca se modifica
    }

    void FixedUpdate()
    {
        // Movimiento con física
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);
    }
}