using UnityEngine;

namespace AE
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager Instance;
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip gameMusic;

        private void Awake()
        {
            TurnOnSingleton();
        }
   
        void Start()
        {
            GameMusic();
        }
        public void GameMusic()
        {
            audioSource.clip = gameMusic;
            audioSource.Play();
        }


        private void TurnOnSingleton()
        {
            // if this already exists, destroy to make sure there is only one.
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }

            //if this is the first one, set the instance to this and put it in DontDestroyOnLoad.
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}
