

namespace AE
{
    using UnityEngine;
    using System.Linq;

    public class QuestArea : MonoBehaviour
    {
        public string[] CorrectQuestItems;
       

        void Start()
        {
            //Sanity check if there are QuestItems added
            foreach (string i in CorrectQuestItems)
                Debug.Log(i);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Collider {other} entered ");
            QuestItem questItem = other.GetComponent<QuestItem>();
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
            Debug.Log("Quest Finished!");
            // Add your quest completion logic here
        }
    }
}
