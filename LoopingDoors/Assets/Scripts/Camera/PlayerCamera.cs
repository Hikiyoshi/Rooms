using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("References"), Space]
    [SerializeField] private Transform orientation;

    [Header("Camera Settings"), Space]
    [SerializeField] private float startXCamera = 0f;
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;
    [SerializeField] private float maxVerticalCameraRotation = 90f;

    private float xRotation;
    private float yRotation;

    private float mouseX;
    private float mouseY;

    private void Start()
    {
        GameManager.Instance.LockCursor();
        yRotation = startXCamera;
    }

    private void Update()
    {
        GetMouseInput();

        HandleMoveCamera();
    }

    private void GetMouseInput()
    {
        mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
    }

    private void HandleMoveCamera()
    {
        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxVerticalCameraRotation, maxVerticalCameraRotation);

        //Rotate Camera and Orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
