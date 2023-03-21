using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SearchAlgorithm : MonoBehaviour
{
   


    private List<Grid> AllSearchedGrids = new List<Grid>();

    private List<List<ColorBlock>> MatchedColorBlocksList = new List<List<ColorBlock>>();
    

    private void OnEnable()
    {
        EventManager.StartListening(GameConstants.GameEvents.GRID_MATRIX_CREATED, GetGridMatrixCreatedMessage);
        EventManager.StartListening(GameConstants.GameEvents.GRID_MATRIX_UPDATED, GetGridMatrixCreatedMessage);
    }

    private void OnDisable()
    {
        EventManager.StopListening(GameConstants.GameEvents.GRID_MATRIX_CREATED, GetGridMatrixCreatedMessage);
        EventManager.StopListening(GameConstants.GameEvents.GRID_MATRIX_UPDATED, GetGridMatrixCreatedMessage);
    }


    private void GetGridMatrixCreatedMessage(EventParam param)
    {
        SearchGridMatrixForMatches(param);
        
            
    }


    

    private void SearchGridMatrixForMatches(EventParam param)
    {
       
        MatchedColorBlocksList.Clear();
        AllSearchedGrids.Clear();
       
        List<List<Grid>> GridMatrix = GridMatrixManager.GetGridMatrix();
        

                
        for (int i = 0; i < GridMatrix.Count;i++)
        {
            for(int j = 0; j < GridMatrix[0].Count;j++)
            {
                Grid grid = GridMatrix[i][j];

                if (!grid.IsEmpty)
                {
                    
                    GameBlock game_block = grid.GetGridBlock();

                   
                    if (game_block.GetBlockType().Equals(GameBlock.BlockType.ColorBlock))
                    {
                        ColorBlock color_block = (ColorBlock)game_block;
                        if (!AllSearchedGrids.Contains(grid))
                        {

                            SearchforNeighbours(color_block, grid, GridMatrix);
                            

                        }
                    }
                }
               
            }
        }
        if(MatchedColorBlocksList.Count == 0)
        {
            SendShuffleMatrixMessage();
        }
        else
        {
            SendSearchCompletedMessage();
        }
        
    }


    private void SendSearchCompletedMessage()
    {
        EventParam search_param = new EventParam(MatchedColorBlocksList);
        EventManager.TriggerEvent(GameConstants.GridMatrixEvents.SEARCH_COMPLETED,search_param);
        




    }

    private void SendShuffleMatrixMessage()
    {
        EventManager.TriggerEvent(GameConstants.GridMatrixEvents.SHUFFLE_MATRIX, new EventParam());

    }


   


    private void SearchforNeighbours(ColorBlock color_block,Grid grid, List<List<Grid>> grid_matrix)
    {

        AllSearchedGrids.Add(grid);
        List<Grid> traversed_grids = new List<Grid>();
        List<ColorBlock> matched_color_blocks = new List<ColorBlock>();
        ColorBlock.ColorBlockTypes color_type = color_block.GetColorType();
        MatrixPoint start_point = grid.GetMatrixPoint();
        int matrix_index_x = start_point.GetMatrixIndexX();
        int matrix_index_y = start_point.GetMatrixIndexY();
        matched_color_blocks.Add(color_block);
        traversed_grids.Add(grid);
        TraverseNew(grid_matrix, traversed_grids, matched_color_blocks, color_type);
        
       
        
        if (matched_color_blocks.Count > 1)
        {
            MatchedColorBlocksList.Add(matched_color_blocks); 
            

        }
        else
        {
            matched_color_blocks[0].SetBlockSprite(0);
        }



    }

    

    void TraverseNew(List<List<Grid>> grid_matrix, List<Grid> traversed_grids, List<ColorBlock> matched_color_blocks,
        ColorBlock.ColorBlockTypes selected_color)
    {
        int row_count = grid_matrix.Count;
        int column_count = grid_matrix[0].Count;

        while (traversed_grids.Count > 0)
        {
            int current_index_x = traversed_grids[0].GetMatrixPoint().GetMatrixIndexX();
            int current_index_y = traversed_grids[0].GetMatrixPoint().GetMatrixIndexY();
            CheckElement(current_index_x - 1, current_index_y,row_count,column_count);
            CheckElement(current_index_x + 1, current_index_y, row_count, column_count);
            CheckElement(current_index_x, current_index_y + 1, row_count, column_count);
            CheckElement(current_index_x, current_index_y - 1,row_count,column_count);

            traversed_grids.Remove(traversed_grids[0]);
        }
        void CheckElement(int x, int y,int row_count, int column_count)
        {
            if (x > -1 && x < row_count && y > -1 && y < column_count)
            {
                GameBlock game_block = grid_matrix[x][y].GetGridBlock();

                if (game_block != null)
                {
                    if (game_block.GetBlockType().Equals(GameBlock.BlockType.ColorBlock))
                    {
                        ColorBlock color_block = (ColorBlock)game_block;
                        Grid traversed_grid = color_block.GetParentGrid();
                        if (!ReferenceEquals(color_block, null) && color_block.GetColorType().Equals(selected_color))
                        {
                            if (!matched_color_blocks.Contains(color_block))
                            {

                                matched_color_blocks.Add(color_block);


                                traversed_grids.Add(traversed_grid);
                                AllSearchedGrids.Add(traversed_grid);

                            }
                        }
                    }
                }

                
            }
        }

    }
}
