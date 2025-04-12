using UnityEngine;

namespace AE
{
    using UnityEngine;

    public class PlayerInteract : MonoBehaviour
    {
        public float distanceToInteractable = 5.0f;
        public Vector3 draggingInteractablePosition = new Vector3(1.0f, 1.0f, 1.0f);
        private GameObject currentInteractableInstance = null;

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
                        // Instantiate the interactable
                        currentInteractableInstance = Instantiate(interactableScript.gameObject);

                        // Set the instantiated object as a child of the player's parent
                        currentInteractableInstance.transform.SetParent(transform.parent);

                        // Set the local position for dragging
                        currentInteractableInstance.transform.localPosition = draggingInteractablePosition;

                        // Disable its Rigidbody if it has one, to prevent physics interference during dragging
                        Rigidbody rb = currentInteractableInstance.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.isKinematic = true;
                        }
                    }
                }
            }

            // Check if the left mouse button is held down and we have an interactable instance
            if (Input.GetMouseButton(0) && currentInteractableInstance != null)
            {
                // Keep the interactable instance at the dragging position relative to the parent
                currentInteractableInstance.transform.localPosition = draggingInteractablePosition;
            }

            // Check for left mouse button up
            if (Input.GetMouseButtonUp(0) && currentInteractableInstance != null)
            {
                // Unparent the interactable instance
                currentInteractableInstance.transform.SetParent(transform.parent.parent);

                // Re-enable its Rigidbody if it had one
                Rigidbody rb = currentInteractableInstance.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                }

                // Reset the current interactable instance
                currentInteractableInstance = null;
            }
        }
    }
}
