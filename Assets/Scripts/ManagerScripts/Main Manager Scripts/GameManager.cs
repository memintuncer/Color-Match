using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerData PlayerData;
    [SerializeField] LoadLevelButton StartLevelButton;
    [SerializeField] private TextMeshProUGUI LevelInfoText, HeartInfoUI, GoldInfoUI, StarCountUI;
    [SerializeField] private GameObject MainScreen;
    [SerializeField] private GameObject AreaObjectPanel,MainScreenButtons;
    private void OnEnable()
    {
        EventManager.StartListening(GameConstants.LEVEL_EVENTS.LEVEL_SUCCESSED, LevelSuccessed);
        EventManager.StartListening(GameConstants.LEVEL_EVENTS.RESTART_LEVEL, RestartLevel);
        EventManager.StartListening(GameConstants.GameEvents.SPAWN_AREA_OBJECT, SpawnAreaObject);
    }

    private void OnDisable()
    {
        EventManager.StopListening(GameConstants.LEVEL_EVENTS.LEVEL_SUCCESSED, LevelSuccessed);
        EventManager.StopListening(GameConstants.LEVEL_EVENTS.RESTART_LEVEL, RestartLevel);
        EventManager.StopListening(GameConstants.GameEvents.SPAWN_AREA_OBJECT, SpawnAreaObject);
    }


    void SpawnAreaObject(EventParam param)
    {
        int req_star_count = param.AreaObjectRequiredStar;
        int current_star_count = PlayerData.StarCount;

        if (current_star_count >= req_star_count)
        {
            PlayerData.StarCount -= req_star_count;
            StarCountUI.text = PlayerData.StarCount.ToString();
            AreaObjectPanel.SetActive(false);
            param.AreaObject.SetActive(true);
            param.AreaObjectButton.SetActive(false);
            MainScreenButtons.SetActive(true);
        }
    }

    void LevelSuccessed(EventParam param)
    {
        MainScreen.SetActive(true);
        int level_count = PlayerData.GetLevelCount()+1;
        PlayerData.SetLevelCount(level_count);
        PlayerData.StarCount++; 
        StartLevelButton.SetLeveLIndex(level_count);
        LevelInfoText.text ="Level " + (level_count+1).ToString();
        StarCountUI.text = PlayerData.StarCount.ToString();
        MainScreenButtons.SetActive(true);

    }

    
    void Start()
    {
        GetPlayerData();
        EventManager.TriggerEvent(GameConstants.GameEvents.GAME_STARTED,new EventParam());
        
    }

    void GetPlayerData()
    {
        int level_index = PlayerData.GetLevelCount();
        StartLevelButton.SetLeveLIndex(level_index);
        LevelInfoText.text = "Level " + (level_index+1).ToString();
        GoldInfoUI.text = PlayerData.GoldCount.ToString();
        HeartInfoUI.text = PlayerData.HeartCount.ToString();
        StarCountUI.text = PlayerData.StarCount.ToString();
    }

    void RestartLevel(EventParam param)
    {
        EventParam level_index_param = new EventParam(PlayerData.GetLevelCount());
        EventManager.TriggerEvent(GameConstants.GameEvents.LOAD_SPECIFIC_LEVEL, level_index_param);
    }



}
