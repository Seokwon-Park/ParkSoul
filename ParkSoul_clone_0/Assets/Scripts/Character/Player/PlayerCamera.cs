using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera instance;
        public PlayerManager player;
        public Camera cameraObject;
        [SerializeField] Transform cameraPivotTransform;

        // change these to tweak camera performance
        [Header("Camera Settings")]
        private float cameraSmoothSpeed = 1; // The bigger this number, the longer for the camera to reach its position during movement
        [SerializeField] float leftAndRightRotationSpeed = 220;
        [SerializeField] float upAndDownRotationSpeed = 220;
        [SerializeField] float minimumPivot = -30; // The lowest point you are able to look down
        [SerializeField] float maximumPivot = 60;  // the highest point you are able to look up
        [SerializeField] float cameraCollisionRadius = 0.2f;
        [SerializeField] LayerMask collideWithLayers;

        // just displays camera values
        [Header("Camera Values")]
        private Vector3 cameraVelocity;
        private Vector3 cameraObjectPosition; // Used for camera collisions(moves the camera object to this position upon colliding)
        [SerializeField] float leftAndRightLookAngle;
        [SerializeField] float upAndDownLookAngle;
        private float cameraZPosition;    // values used for camera collisions
        private float targetCameraZPosition;     // values used for camera collisions    

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
            cameraZPosition = cameraObject.transform.localPosition.z;
        }

        public void HandleAllCameraActions()
        {
            if (player != null)
            {
                // FOLLOW THE PLAYER
                HandleFollowTarget();
                // ROTATE AROUND THE PLAYER
                HandleRotations();
                // COLLIDE WITH OBJECTS
                HandleCollisions();
            }
        }

        private void HandleFollowTarget()
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;
        }

        private void HandleRotations()
        {
            // if locked on, force rotation towards target
            // else rotate reguarly

            // normal rotations
            // rotate left and right based on horizontal movement on the right joystick
            leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
            // rotate up and down based on vertical movement on the right joystick
            upAndDownLookAngle -= (PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;
            // clame the up and down angle between a min and max value
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

            Vector3 cameraRotation = Vector3.zero;
            Quaternion targetRotation;

            // Rotate this gameobject left and right
            cameraRotation.y = leftAndRightLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;

            // rotate the pivot gameobject up and down
            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotTransform.localRotation = targetRotation;
        }

        private void HandleCollisions()
        {
            targetCameraZPosition = cameraZPosition;
            RaycastHit hit;
            Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
            direction.Normalize();

            // We Check if there is an object in front of our desired direction ^ (see above)
            if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
            {
                // if there is, we get our distance from it
                float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
                // we then equate our target z position to the following 
                targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
            }

            // if our target position is less than our collision radius, we subtract our collision radius ( making it snap back)
            if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
            {
                targetCameraZPosition = -cameraCollisionRadius;
            }

            // we than apply our final position using a lerp over a time of 0.2f
            cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
            cameraObject.transform.localPosition = cameraObjectPosition;
        }
    }
}