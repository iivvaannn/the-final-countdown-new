using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor; 

public class ConversationStarter : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collide");
            if(Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("PressE");
                ConversationManager.Instance.StartConversation(myConversation);  
            }
        }
    }
}
