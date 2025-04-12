
namespace AE
{
    using UnityEngine;

    public class PlayerInteract : MonoBehaviour
    {
        public float distanceToInteractable = 5.0f;
        public Vector3 draggingInteractablePosition; // Defaults in Start()
        private GameObject currentInteractableInstance = null;
        private GameObject originalInteractable = null;

        void Start()
        {
            // Default draggingInteractablePosition to the current global position
            draggingInteractablePosition = transform.position;
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

                        // Instantiate the interactable at the dragging position
                        currentInteractableInstance = Instantiate(originalInteractable, draggingInteractablePosition, Quaternion.identity);

                        // Set the instantiated object as a child of this GameObject
                        currentInteractableInstance.transform.SetParent(transform);

                        // Set the local position for dragging (relative to this GameObject)
                        currentInteractableInstance.transform.localPosition = Vector3.zero; // Attach at the origin

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
                // The instance moves along with this GameObject because it's a child
                // We can optionally update its local position if needed for visual adjustments
                currentInteractableInstance.transform.localPosition = Vector3.zero;
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
