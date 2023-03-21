using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerData
{
    public int StarCount=0;
    public int LevelCount=0;
    public int AreaCount=0;
    public int GoldCount = 0;
    public int HeartCount = 5;

    public int GetStarCount()
    {
        return StarCount;
    }

    public void LoadPlayerData()
    {

    }

    public int GetLevelCount()
    {
        return LevelCount;
    }

    public void SetLevelCount(int level_count)
    {
        LevelCount = level_count;
    }

}
