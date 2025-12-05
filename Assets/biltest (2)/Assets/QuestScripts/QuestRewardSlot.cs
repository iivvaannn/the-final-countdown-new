using UnityEngine;
using TMPro;

public class QuestRewardSlot : MonoBehaviour
{
    public TextMeshProUGUI rewardDescriptionText;
    public TextMeshProUGUI goldRewardText;

    private Quest activeQuest;

    // Använd för att sätta questen i sloten, t.ex. från QuestManager
    public void SetQuest(Quest quest)
    {
        activeQuest = quest;

        if (rewardDescriptionText != null)
            rewardDescriptionText.text = activeQuest.description;

        if (goldRewardText != null)
            goldRewardText.text = activeQuest.goldReward + " Gold";
    }

    // Kallas när spelaren slutför questen och vill få belöningen
    public void ClaimReward()
    {
        if (activeQuest != null && !activeQuest.isCompleted)
        {
            QuestManager.Instance.CompleteQuest();

            // Här lägger vi till guldet via GoldBank singleton
            GoldBank.Instance.AddGold(activeQuest.goldReward);

            Debug.Log("Quest belöning hämtad: " + activeQuest.goldReward + " gold");
        }
        else
        {
            Debug.Log("Ingen aktiv quest eller quest redan slutförd.");
        }
    }
}
