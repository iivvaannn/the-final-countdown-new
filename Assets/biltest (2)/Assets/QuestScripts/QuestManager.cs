using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public Quest activeQuest;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AcceptQuest(Quest quest)
    {
        if (activeQuest == null || activeQuest.isCompleted)
        {
            activeQuest = quest;
            Debug.Log("Tog uppdrag: " + quest.questName);
        }
        else
        {
            Debug.LogWarning("Kan inte acceptera nytt quest, redan ett aktivt quest!");
        }
    }

    public void CompleteQuest()
    {
        if (activeQuest != null && !activeQuest.isCompleted)
        {
            activeQuest.isCompleted = true;
            Debug.Log("Quest completed: " + activeQuest.questName);

            // Exempel på belöningshantering
            GoldBank.Instance.AddGold(activeQuest.goldReward);
            Debug.Log("Guld efter addition: " + GoldBank.Instance.currentGold);

            activeQuest = null;  // Rensa aktiv quest när den är klar
        }
        else
        {
            Debug.LogWarning("CompleteQuest kallades men quest är null eller redan slutförd");
        }
    }
}
