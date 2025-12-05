using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest System/Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    public string description;
    public bool isCompleted;
    public Vector3 targetPosition;
    public int goldReward;
}
