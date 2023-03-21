using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    [SerializeField] private GameObject[] GameLevelPrefabs;
    [SerializeField] private Transform GameScreenTransform;
    public GameObject CurrentLevel;
    public int Level_Index;

    private void OnEnable()
    {
        EventManager.StartListening(GameConstants.GameEvents.LOAD_SPECIFIC_LEVEL, LoadSpecificLevel);
        EventManager.StartListening(GameConstants.LEVEL_EVENTS.LEVEL_SUCCESSED, DestroyLevel);

    }

    private void OnDisable()
    {
        EventManager.StopListening(GameConstants.GameEvents.LOAD_SPECIFIC_LEVEL, LoadSpecificLevel);
        EventManager.StopListening(GameConstants.LEVEL_EVENTS.LEVEL_SUCCESSED, DestroyLevel);

    }

    void DestroyLevel(EventParam param)
    {
        Destroy(CurrentLevel);
    }
    void LoadSpecificLevel(EventParam level_index_param)
    {
        if(CurrentLevel != null)
        {
            Destroy(CurrentLevel);
        }
        int Level_Index = level_index_param.GetLevelIndex()%GameLevelPrefabs.Length;
        GameObject game_level = Instantiate(GameLevelPrefabs[Level_Index], Vector2.zero, Quaternion.identity, GameScreenTransform);
        CurrentLevel = game_level;
    }

    

}
