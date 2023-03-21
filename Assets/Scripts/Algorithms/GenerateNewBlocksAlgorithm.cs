using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateNewBlocksAlgorithm : MonoBehaviour
{

    [SerializeField] private float StartingPositionYValue;
    [SerializeField] private GameplayManager GamePlayManager;
    private void OnEnable()
    {
        EventManager.StartListening(GameConstants.GameEvents.GENEREATE_NEW_GAME_BLOCKS_FOR_EMPTY_GRIDS, GenerateGameBlocks);
    }

    private void OnDisable()
    {
        EventManager.StopListening(GameConstants.GameEvents.GENEREATE_NEW_GAME_BLOCKS_FOR_EMPTY_GRIDS, GenerateGameBlocks);
    }
  

    private void GenerateGameBlocks(EventParam param)
    {
        Dictionary<int, int> MatchedColumns = param.GetMatchedColumns();
        ColorBlock.ColorBlockTypes[] LevelSpecificColorBlocks = GamePlayManager.GetLevelSpecificColorBlocks();
        List<List<Grid>> grid_matrix = GridMatrixManager.GetGridMatrix();

        

        foreach (int key in MatchedColumns.Keys)
        {
            int empty_grid_count = MatchedColumns[key];
            int row_index = grid_matrix[0].Count- empty_grid_count;
            for (int i=0;i<empty_grid_count;i++)
            {
                Grid empty_grid = grid_matrix[key][row_index];
              
                
                row_index++;
                GameObject new_color_block_object = GetRandomColorBlockFromPool(LevelSpecificColorBlocks);
                ColorBlock color_block = new_color_block_object.GetComponent<ColorBlock>();
                new_color_block_object.transform.position = grid_matrix[key][row_index - 1].transform.position + new Vector3(0, 10,0);

                new_color_block_object.SetActive(true);
                new_color_block_object.transform.parent = empty_grid.transform;
                new_color_block_object.transform.localScale =Vector3.one;
                AssignGameBlockToGrid(empty_grid, color_block);
                color_block.Sliding = true;
            }
        }

        EventManager.TriggerEvent(GameConstants.GameEvents.GRID_MATRIX_UPDATED, new EventParam());
    }


    GameObject GetRandomColorBlockFromPool(ColorBlock.ColorBlockTypes[] level_specific_color_blocks)
    {
        
        int random_block_index = Random.Range(0, level_specific_color_blocks.Length);
        GameObject color_block = PoolingManager.GetColorBlockFromPool(level_specific_color_blocks[random_block_index]);
        
        return color_block;
    }

    void AssignGameBlockToGrid(Grid grid, GameBlock game_block)
    {
        game_block.SetSpriteSortingOrder(grid.GetSpriteOrderLevel());
        game_block.transform.parent = grid.transform;
        
        game_block.gameObject.SetActive(true);
        grid.SetGridBlock(game_block);

    }
}
