using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadLevelButton : MonoBehaviour
{
    private int LevelIndex=0;

    public void SetLeveLIndex(int level_index)
    {
        LevelIndex = level_index;
    }
    
    void Start()
    {
        
    }

    public void SendLoadLevelMessage()
    {
        EventParam level_index_param = new EventParam(LevelIndex);
        EventManager.TriggerEvent(GameConstants.GameEvents.LOAD_SPECIFIC_LEVEL, level_index_param);
    }

    
}
