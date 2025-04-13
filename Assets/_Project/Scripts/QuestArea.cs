

namespace AE
{
    using UnityEngine;
    using System.Linq;
    using DG.Tweening;

    public class QuestArea : MonoBehaviour
    {
        public string[] CorrectQuestItems;
        public GameObject Door; // Assign the door GameObject in the Inspector
        public GameObject NotificationWindow; //TODO: Get rid of this logic once TMPro is repaired
        public GameObject QuestNotificationWindow; //TODO: Get rid of this logic once TMPro is repaired
        private bool isQuestFinished = false;

        void Start()
        {
            DOTween.Init();
            //Sanity check if there are QuestItems added
            foreach (string i in CorrectQuestItems)
                Debug.Log(i);
            //QuestFinished(); //TODO: just for test, turn off!
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Collider {other} entered ");
            QuestItem questItem = other.GetComponent<QuestItem>();
            if (isQuestFinished == false)
            {
            QuestNotificationWindow.SetActive(true);
            NotificationWindow.SetActive(false);
            }
            if (questItem != null)
            {
                string itemName = questItem.QuestItemName;

                // Check if the QuestItemName exists in the CorrectQuestItems array
                int index = System.Array.IndexOf(CorrectQuestItems, itemName);

                if (index >= 0)
                {
                    Debug.Log($"Correct item '{itemName}' found in QuestArea.");

                    // Create a new array without the found item
                    CorrectQuestItems = CorrectQuestItems.Where((source, i) => i != index).ToArray();


                    // Destroy the QuestItem that was placed
                    Destroy(other.gameObject);

                    // Check if all quest items have been collected
                    if (CorrectQuestItems.Length == 0)
                    {
                        QuestFinished();
                    }
                }
                else
                {
                    Debug.Log($"Incorrect item '{itemName}' placed in QuestArea.");
                    // You might want to provide feedback to the player here
                }
            }
        }

        private void QuestFinished()
        {
            isQuestFinished = true;
            // Tween the door's rotation if the Door GameObject is assigned
            if (Door != null)
            {
                QuestNotificationWindow.SetActive(false);
                //TODO: Add sound
                //TODO: Add some light to the door?

                //Debug.Log(Door.transform.rotation); //check before rotation
                Door.transform.DORotate(new Vector3(0f, 107f, 0f), 3f, RotateMode.LocalAxisAdd)
                    .SetEase(Ease.Linear); // Tween over 3 seconds with a linear ease curve
                //Debug.Log(Door.transform.rotation); //check after rotation
                Debug.Log("Quest Finished, door opening");
                

            }
            else
            {
                Debug.LogWarning("Quest Finished, but Door GameObject is not assigned. Cannot tween rotation.");
            }

            // Add any other quest completion logic here
        }
    }
}
