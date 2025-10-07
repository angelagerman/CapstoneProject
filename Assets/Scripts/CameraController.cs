using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;            // The player to follow
    public Transform cameraTransform;   // The actual camera
    public Vector3 offset = new Vector3(0, 2, -4); // Camera offset (behind and above)
    public float mouseSensitivity = 2f;
    public float followSpeed = 10f;
    public float minYAngle = -30f;      // Prevents looking too far up
    public float maxYAngle = 60f;       // Prevents looking too far down
    
    public float minZoom = 2f;
    public float maxZoom = 10f;
    public float zoomSpeed = 2f;

    public LayerMask collisionLayer;

    private float yaw;  // Horizontal rotation (left/right)
    private float pitch; // Vertical rotation (up/down)
    //haha airplane terms i took rocket science in highschool i know what that is

    void Start()
    {
        if (!cameraTransform)
        {
            cameraTransform = Camera.main.transform;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (!target) return;

        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minYAngle, maxYAngle);
        
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        offset.z += -scroll * zoomSpeed;
        offset.z = Mathf.Clamp(offset.z, -maxZoom, -minZoom);
        
        transform.position = Vector3.Lerp(transform.position, target.position, followSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Position the camera relative to the target
        Vector3 desiredCameraPos = transform.position + transform.rotation * offset;
        
        Vector3 origin = transform.position + Vector3.up * 1.5f; // Start casting from head height
        Vector3 direction = desiredCameraPos - origin;
        float distance = direction.magnitude;
        
        RaycastHit hit;
        if (Physics.SphereCast(origin, 0.3f, direction.normalized, out hit, distance, collisionLayer))
        {
            cameraTransform.position = origin + direction.normalized * (hit.distance - 0.05f); // Offset to avoid clipping
        }
        else
        {
            cameraTransform.position = desiredCameraPos;
        }
        cameraTransform.LookAt(transform.position + Vector3.up * 1.5f); // Optional look-at target offset
    }
    
}
