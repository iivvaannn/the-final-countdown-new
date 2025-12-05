using UnityEngine;

public class QuestTarget : MonoBehaviour
{
    public GameObject weaponPrefab;     // Endast för "Find the Weapon"-quest
    public Transform spawnPoint;        // Var vapnet ska spawnas

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && QuestManager.Instance.activeQuest != null)
        {
            Quest quest = QuestManager.Instance.activeQuest;

            if (!quest.isCompleted)
            {
                Debug.Log("Spelaren nådde mål för: " + quest.questName);
                QuestManager.Instance.CompleteQuest();

                // Om quest är "Find the Weapon" spawnar vi vapnet
                if (weaponPrefab != null && quest.questName == "Find the Weapon")
                {
                    if (spawnPoint != null)
                    {
                        Instantiate(weaponPrefab, spawnPoint.position, Quaternion.identity);
                        Debug.Log("Vapen spawnat.");
                    }
                    else
                    {
                        Debug.LogWarning("spawnPoint saknas på QuestTarget!");
                    }
                }

                Destroy(gameObject); // Ta bort målobjektet
            }
        }
    }
}
