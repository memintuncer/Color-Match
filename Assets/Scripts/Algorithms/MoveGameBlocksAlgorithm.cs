using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveGameBlocksAlgorithm : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.StartListening(GameConstants.GameEvents.MATCHED_REMOVED, MoveGameBlocks);
    }

    private void OnDisable()
    {
        EventManager.StopListening(GameConstants.GameEvents.MATCHED_REMOVED, MoveGameBlocks);
    }






   private void MoveGameBlocks(EventParam param)
   {
        Dictionary<int, int> RemovedBlockIndexCounts = param.GetMatchedColumns();
        List<int> columns = new List<int>(RemovedBlockIndexCounts.Keys);
        List<List<Grid>> grid_matrix = GridMatrixManager.GetGridMatrix();
        int row_count = grid_matrix.Count;
        

        foreach (int col in columns)
        {
           
            int slide_count = 0;
            
            for (int row = 0; row < row_count; row++)
            {
                

                Grid falling_grid = grid_matrix[col][row];

                if (falling_grid.IsEmpty)
                {
                    slide_count++;
                }

                if(!falling_grid.IsEmpty && slide_count != 0)
                {
                    
                    Grid target_grid = grid_matrix[col][row - slide_count];
                    GameBlock game_block = falling_grid.GetGridBlock();

                    if (game_block.GetCubeCanMove())
                    {
                        falling_grid.RemoveGameBlockFromGrid();
                        target_grid.SetGridBlock(game_block);
                        game_block.transform.parent = target_grid.transform;
                        game_block.Sliding = true;
                        game_block.SetSpriteSortingOrder(target_grid.GetSpriteOrderLevel());

                       
                    }
                    else
                    {
                        RemovedBlockIndexCounts[col] -= slide_count;
                        slide_count = 0;

                    }
                    

                }



            }


        }

       SendGenerateNewBlocksMessage(param);

    }

    
    private void SendGenerateNewBlocksMessage(EventParam param)
    {
        EventManager.TriggerEvent(GameConstants.GameEvents.GENEREATE_NEW_GAME_BLOCKS_FOR_EMPTY_GRIDS, param);
    }

}

