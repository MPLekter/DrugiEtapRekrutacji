using UnityEngine;
using DG.Tweening;


namespace AE
{
    public class OpenCloseLid : MonoBehaviour
    {
        [Header("Tween Settings")]
        public float duration = 2.5f;
        public Ease easeType = Ease.InOutSine;
        public LoopType loopType = LoopType.Yoyo;
        public int loops = -1; // -1 for infinite loops

        [Header("Target Positions")]
        public Vector3 positionA = new Vector3(0f, 0f, 0f);
        public Vector3 positionB = new Vector3(-60f, 0f, 0f);

        // Start is called before the first frame update
        void Start()
        {
            transform.DORotate(positionB, duration)
            .SetEase(easeType)
            .SetLoops(loops, loopType)
            .From(positionA);
        }

    }
}
