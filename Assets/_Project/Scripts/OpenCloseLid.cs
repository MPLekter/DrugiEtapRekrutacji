using UnityEngine;

namespace AE
{
    public class OpenCloseLid : MonoBehaviour
    {
        [SerializeField] Vector3 rotationSpeed;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(rotationSpeed);
            
        }
    }
}
