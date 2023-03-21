using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Messaging System
//When classes complete their own transactions, they can send messages through this script. 
//Other classes listening to the required message perform their own operations according to the incoming message.

public class EventManager : MonoBehaviour
{
 
    private Dictionary<string, Action<EventParam>> eventDictionary;

    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }
            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, Action<EventParam>>();
        }
    }

    public static void StartListening(string eventName, Action<EventParam> listener)
    {
        Action<EventParam> thisEvent;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            
            thisEvent += listener;

           
            instance.eventDictionary[eventName] = thisEvent;
        }
        else
        {
            
            thisEvent += listener;
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, Action<EventParam> listener)
    {
        if (eventManager == null) return;
        Action<EventParam> thisEvent;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
           
            thisEvent -= listener;

           
            instance.eventDictionary[eventName] = thisEvent;
        }
    }

    public static void TriggerEvent(string eventName, EventParam eventParam)
    {
        Action<EventParam> thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(eventParam);
            
        }
    }
}


public class EventParam
{


    private List<List<Grid>> GridMatrix = new List<List<Grid>>();

    private int LevelIndex;
    private ColorBlock SelectedColorBlock;
    private List<List<ColorBlock>> MatchedColorBlocks = new List<List<ColorBlock>>();
    private Dictionary<int, int> MatchedColumns = new Dictionary<int, int>();
    private Dictionary<int, List<int>> RemovedBlockIndexes = new Dictionary<int, List<int>>();

    private List<ColorBlock> RemovedColorBlocks = new  List<ColorBlock>();
    private List<SpecialBlock> RemovedSpecialBlocks = new  List<SpecialBlock>();

    private GameObject areaObject;
    
    public GameObject AreaObject
    { 
        get { return areaObject; }
        set { areaObject = value; }
    }
    private int areaObjectRequiredStar;

    public int AreaObjectRequiredStar
    {
        get { return areaObjectRequiredStar; }
        set { areaObjectRequiredStar = value; }
    }

    private GameObject areaObjectButton;

    public GameObject AreaObjectButton
    {
        get { return areaObjectButton; }
        set { areaObjectButton = value; }
    }


    public List<List<Grid>> GetGridMatrix()
    {
        return GridMatrix;
    }

    public void SetGridMatrix(List<List<Grid>> grid_matrix)
    {
        GridMatrix = grid_matrix;
    }

    public int GetLevelIndex()
    {
        return LevelIndex;
    }

    public void SetLevelIndex(int level_index)
    {
        LevelIndex = level_index;
    }

  
    public List<List<ColorBlock>> GetMatchedColorBlocks()
    {
        return MatchedColorBlocks;
    }

    public void SetMatchedColorBlocks(List<List<ColorBlock>> matched_color_blocks)
    {
        MatchedColorBlocks = matched_color_blocks;
    }


    public ColorBlock GetSelectedColorBlock()
    {
        return SelectedColorBlock;
    }

    public void SetSelectedColorBlock(ColorBlock color_block)
    {
        SelectedColorBlock = color_block;
    }


    public  Dictionary<int, int> GetMatchedColumns()
    {
        return MatchedColumns;
    }

    public void SetMatchedColumns(Dictionary<int, int> removed_blockindex_counts)
    {
        MatchedColumns = removed_blockindex_counts;
    }

   
    public List<ColorBlock> GetRemovedColorBlocks()
    {
        return RemovedColorBlocks;
    }

    public void SetRemovedColorBlocks(List<ColorBlock> removed_color_blocks)
    {
        RemovedColorBlocks = removed_color_blocks;
    }



    public List<SpecialBlock> GetRemovedSpecialBlocks()
    {
        return RemovedSpecialBlocks;
    }

    public void SetRemovedSpecialBlocks(List<SpecialBlock> removed_special_blocks)
    {
        RemovedSpecialBlocks = removed_special_blocks;
    }




    public EventParam()
    {

    }
    public EventParam(List<ColorBlock> removed_color_blocks)
    {
        this.RemovedColorBlocks = removed_color_blocks;
    }

    public EventParam(int level_index)
    {
        this.LevelIndex = level_index;
    }

    public EventParam(List<List<ColorBlock>> matched_color_blocks)
    {
        this.MatchedColorBlocks = matched_color_blocks;
    }

    public EventParam(ColorBlock selected_color_block)
    {
        this.SelectedColorBlock = selected_color_block;
    }

}



