using UnityEngine;

public class Player_camera : MonoBehaviour
{

    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    public Rigidbody body;
    public Transform playerTransform;

    [SerializeField] private float xRotation;
    [SerializeField] private float yRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Mouvement avec la souris
        float mousX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mousY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mousX;
        xRotation -= mousY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerTransform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}