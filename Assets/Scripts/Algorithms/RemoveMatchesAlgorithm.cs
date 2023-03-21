using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveMatchesAlgorithm : MonoBehaviour
{

    private Dictionary<int, int> MatchedColumns = new Dictionary<int, int>();
   
    public int Count = 0;
    private void OnEnable()
    {
        EventManager.StartListening(GameConstants.GameEvents.REMOVE_MATCHED_COLOR_BLOCKS, RemoveMatchedColorBlocks);
    }

    private void OnDisable()
    {
        EventManager.StopListening(GameConstants.GameEvents.REMOVE_MATCHED_COLOR_BLOCKS, RemoveMatchedColorBlocks);
    }

    private void RemoveMatchedColorBlocks(EventParam param)
    {
       
        ColorBlock selected_color_block = param.GetSelectedColorBlock();
        List<List<ColorBlock>> matched_color_blocks = param.GetMatchedColorBlocks();

        int count = matched_color_blocks.Count;

        for(int i =0; i < count; i++)
        {
            List<ColorBlock> matched_groups = matched_color_blocks[i];
            if (matched_groups.Contains(selected_color_block))
            {
                
                SendCheckForGameGoalMessage(matched_groups);
                SendGameBlocksToPool(matched_groups);
                break;
            }
        }

       
        SendMoveGameBlocksMessage(param);

        MatchedColumns.Clear();
        
    }

    void SendCheckForGameGoalMessage(List<ColorBlock> matched_groups)
    {
        EventParam param = new EventParam(matched_groups);
        EventManager.TriggerEvent(GameConstants.GameEvents.CHECK_FOR_GAME_GOAL, param);
    }
    private void SendMoveGameBlocksMessage(EventParam param)
    {
        Count++;
        param.SetMatchedColumns(MatchedColumns);
        
        EventManager.TriggerEvent(GameConstants.GameEvents.MATCHED_REMOVED, param);
    }

    private void SendGameBlocksToPool(List<ColorBlock> matched_groups)
    {
        for(int i = 0; i < matched_groups.Count; i++)
        {
            ColorBlock color_block = matched_groups[i];
            UptadeEmptyGridDict(color_block);
            PoolingManager.SendColorBlockBackToPool(matched_groups[i]);



        }
    }

    private void UptadeEmptyGridDict(ColorBlock color_block)
    {
        Grid grid = color_block.GetParentGrid();
        MatrixPoint point = grid.GetMatrixPoint();
        int index_x = point.GetMatrixIndexX();
        int index_y = point.GetMatrixIndexY();

        
        if (MatchedColumns.ContainsKey(index_x))
        {
            MatchedColumns[index_x]++;
            

        }
        else
        {
            MatchedColumns.Add(index_x, 1);
           
        }



    }
}
