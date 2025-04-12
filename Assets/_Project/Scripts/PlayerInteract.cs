
namespace AE
{
    using UnityEngine;

    public class PlayerInteract : MonoBehaviour
    {
        public float distanceToInteractable = 5.0f;
        public Vector3 draggingInteractablePositionOffset = new Vector3(0f, 0f, 0f); // Initial offset
        public float swingFrequencySide = 1f;
        public float swingAmplitudeSide = 1f;
        public float swingFrequencyForward = 0.75f;
        public float swingAmplitudeForward = 0.5f;
        public float swingFrequencyUp = 0.5f;
        public float swingAmplitudeUp = 0.125f;

        private GameObject currentInteractableInstance = null;
        private GameObject originalInteractable = null;
        private float dragStartTime;
        private Vector3 initialLocalPosition;

        void Start()
        {
            // Default draggingInteractablePosition to the current global position + offset
            draggingInteractablePositionOffset = Vector3.zero; // Initialize without explicit offset here
        }

        void Update()
        {
            // Check for left mouse button down
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, distanceToInteractable))
                {
                    Interactable interactableScript = hit.collider.GetComponent<Interactable>();
                    if (interactableScript != null)
                    {
                        // Store the original GameObject
                        originalInteractable = interactableScript.gameObject;

                        // Calculate the initial global position
                        Vector3 initialPosition = transform.position + draggingInteractablePositionOffset;

                        // Instantiate the interactable at the initial position
                        currentInteractableInstance = Instantiate(originalInteractable, initialPosition, Quaternion.identity);

                        // Set the instantiated object as a child of this GameObject
                        currentInteractableInstance.transform.SetParent(transform);

                        // Store the initial local position relative to the parent
                        initialLocalPosition = currentInteractableInstance.transform.localPosition;

                        // Store the time when dragging started
                        dragStartTime = Time.time;

                        // Disable its Rigidbody if it has one
                        Rigidbody rb = currentInteractableInstance.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.isKinematic = true;
                        }

                        // Destroy the original GameObject
                        Destroy(originalInteractable);
                    }
                }
            }

            // Check if the left mouse button is held down and we have an interactable instance
            if (Input.GetMouseButton(0) && currentInteractableInstance != null)
            {
                // Calculate the swinging motion
                float timeElapsed = Time.time - dragStartTime;

                float swingSide = Mathf.Sin(timeElapsed * swingFrequencySide) * swingAmplitudeSide;
                float swingForward = Mathf.Sin(timeElapsed * swingFrequencyForward) * swingAmplitudeForward;
                float swingUp = Mathf.Sin(timeElapsed * swingFrequencyUp) * swingAmplitudeUp * 0.25f; // Very gentle up and down

                // Calculate the local offset with the swing
                Vector3 localOffset = initialLocalPosition + new Vector3(swingSide, swingUp, swingForward * 0.5f); // Gentle back and forth

                // Set the local position
                currentInteractableInstance.transform.localPosition = localOffset;
            }

            // Check for left mouse button up
            if (Input.GetMouseButtonUp(0) && currentInteractableInstance != null)
            {
                // Unparent the interactable instance, making it a sibling in the scene
                currentInteractableInstance.transform.SetParent(null);

                // Re-enable its Rigidbody if it had one
                Rigidbody rb = currentInteractableInstance.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                }

                // Reset the current interactable instance and original reference
                currentInteractableInstance = null;
                originalInteractable = null;
            }
        }
    }
}
