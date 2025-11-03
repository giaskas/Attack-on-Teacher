using UnityEngine;

public class PlayerCamara : MonoBehaviour
{
    public static PlayerCamara instance;
    public PlayerManager player;
    public Camera cameraObject;
    [SerializeField] Transform cameraPivotTransform;

    [Header("Configuracion de camara")]
    public Vector3 camaraOffset = new Vector3(0, 0, -3);
    private float camaraSmoothSpeed = 0.1f;
    [SerializeField] float leftAndRightRotationSpeed = 220;
    [SerializeField] float upAndDownRotationSpeed = 220;
    [SerializeField] float minimumPivot = -30;
    [SerializeField] float maximumPivot = 60;
    [SerializeField] float cameraCollisionRadius = 0.2f;
    [SerializeField] LayerMask collideWithLayers;

    [Header("valores de la camara")]
    private Vector3 cameraVelocity;
    private Vector3 cameraObjectPosition; // Usado en HandleCollision
    [SerializeField] float leftAndRightLookAngle;
    [SerializeField] float upAndDownLookAngle;
    private float cameraZPosition;
    private float targetCameraZPosition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (cameraObject != null)
        {
            cameraObject.transform.localPosition = camaraOffset;
        }
        else
        {
            Debug.LogError("PlayerCamara: No has asignado el 'cameraObject' en el Inspector!");
            return;
        }

        cameraZPosition = cameraObject.transform.localPosition.z;
        targetCameraZPosition = cameraZPosition; 
    }

    private void LateUpdate()
    {
        if (player == null)
        {
            return; 
        }

        HandleAllCameraActions();
    }

    public void HandleAllCameraActions()
    {
        HandleFollowTarget();
        HandleRotations();
        HandleCollision();
    }

    private void HandleFollowTarget()
    {
        Vector3 targetPosition = player.transform.position;
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref cameraVelocity,
            camaraSmoothSpeed
        );
    }

    private void HandleRotations()
    {
        leftAndRightLookAngle += (PlayerInputManager.instance.camaraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
        upAndDownLookAngle -= (PlayerInputManager.instance.camaraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;
        upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

        Vector3 cameraRotation = Vector3.zero;
        Quaternion targetRotation;

        cameraRotation.y = leftAndRightLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;

        cameraRotation = Vector3.zero;
        cameraRotation.x = upAndDownLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        cameraPivotTransform.localRotation = targetRotation;
    }
    
    private void HandleCollision()
    {
        targetCameraZPosition = cameraZPosition;
        RaycastHit hit;

        Vector3 pivotPosition = cameraPivotTransform.position;
        Vector3 idealCameraWorldPos = cameraPivotTransform.TransformPoint(new Vector3(0, 0, cameraZPosition));
        Vector3 direction = (idealCameraWorldPos - pivotPosition).normalized;


        Vector3 startPoint = pivotPosition + (direction * 1.2f); 
        
        if (Physics.Linecast(startPoint, idealCameraWorldPos, out hit, collideWithLayers))
        {
            Vector3 newCameraWorldPos = hit.point + (hit.normal * cameraCollisionRadius);
            Vector3 newCameraLocalPos = cameraPivotTransform.InverseTransformPoint(newCameraWorldPos);
            targetCameraZPosition = newCameraLocalPos.z;
        }

        if (targetCameraZPosition > -cameraCollisionRadius)
        {
            targetCameraZPosition = -cameraCollisionRadius;
        }

        cameraObjectPosition = cameraObject.transform.localPosition;
        cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
        cameraObject.transform.localPosition = cameraObjectPosition;
    }   
    
}