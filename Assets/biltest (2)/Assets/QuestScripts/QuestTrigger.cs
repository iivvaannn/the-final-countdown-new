using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestTrigger : MonoBehaviour
{
    public GameObject pressEUI;           // "Press E to start quest"
    public GameObject questPanelUI;       // Quest-panelen med info och acceptknapp

    public TextMeshProUGUI questNameText;        // Text för questnamn i questPanelUI
    public TextMeshProUGUI questDescriptionText; // Text för beskrivning
    public TextMeshProUGUI questRewardText;      // Text för belöning, t.ex. "Reward: 20 Gold"
    public Button acceptButton;                    // Accept-knappen

    public Quest quest;                           // Quest ScriptableObject som denna trigger ger

    private bool isPlayerInZone = false;

    private void Start()
    {
        pressEUI.SetActive(false);
        questPanelUI.SetActive(false);

        acceptButton.onClick.AddListener(AcceptQuest);
    }

    private void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.E))
        {
            OpenQuestPanel();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            pressEUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            pressEUI.SetActive(false);
            questPanelUI.SetActive(false);
        }
    }

    void OpenQuestPanel()
    {
        pressEUI.SetActive(false);

        // Visa panel och uppdatera texter
        questPanelUI.SetActive(true);
        questNameText.text = quest.questName;
        questDescriptionText.text = quest.description;
        questRewardText.text = "Reward: " + quest.goldReward + " Gold";
    }

   public void AcceptQuest()
    {
        QuestManager.Instance.AcceptQuest(quest);

        // Dölj paneler efter acceptans
        questPanelUI.SetActive(false);
        pressEUI.SetActive(false);
    }
}
