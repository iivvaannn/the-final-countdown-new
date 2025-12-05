using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    private bool playerInRange = false;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            QuestManager.Instance.AcceptQuest(quest);
            Debug.Log("Aktivt quest är nu: " + (QuestManager.Instance.activeQuest != null ? QuestManager.Instance.activeQuest.questName : "Inget"));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Spelaren är nära NPC med quest: " + quest.questName);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Spelaren lämnade NPC med quest: " + quest.questName);
        }
    }
}
