using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMatrixManager : MonoBehaviour
{
    [SerializeField] int RowCount, ColumnCount;
    private static List<List<Grid>> GridMatrix = new List<List<Grid>>();
    [SerializeField] float GridPositionIndicator;
    public float GridSizeScaler;
    private int TotalGridCount=0;
    [SerializeField] private GameObject GridPrefab;
    [SerializeField] Transform GridMatrixParent;
    public GameplayManager GamePlayManager;


    private void OnEnable()
    {
        EventManager.StartListening(GameConstants.GridMatrixEvents.SHUFFLE_MATRIX, ShuffleMatrix);
    }

    private void OnDisable()
    {
        EventManager.StopListening(GameConstants.GridMatrixEvents.SHUFFLE_MATRIX, ShuffleMatrix);
    }


    


    private void ShuffleMatrix(EventParam param)
    {
        
        System.Random randomizer = new System.Random();

        int n = GridMatrix.Count;
        while (n > 1)
        {
            n--;
            int k = randomizer.Next(n + 1);
            List<Grid> grid = GridMatrix[k];
            GridMatrix[k] = GridMatrix[n];
            GridMatrix[n] = grid;
        }

        foreach (var sublist in GridMatrix)
        {
            n = sublist.Count;
            while (n > 1)
            {
                n--;
                int k = randomizer.Next(n + 1);

                Grid grid_1 = sublist[k];
                Grid grid_2 = sublist[n];
                GameBlock game_block_1 = grid_1.GetGridBlock();
                GameBlock game_block_2 = grid_2.GetGridBlock();

                grid_1.RemoveGameBlockFromGrid();
                grid_2.RemoveGameBlockFromGrid();

                grid_1.SetGridBlock(game_block_2);
                game_block_2.transform.parent = grid_1.transform;
                game_block_2.Sliding = true;
                game_block_2.SetSpriteSortingOrder(grid_1.GetSpriteOrderLevel());
                grid_2.SetGridBlock(game_block_1);
                game_block_1.transform.parent = grid_2.transform;
                game_block_1.Sliding = true;
                game_block_1.SetSpriteSortingOrder(grid_2.GetSpriteOrderLevel());

               
            }
        }
        SendGridMatrixCreatedMessage();

    }


   

    public int GetRowCount()
    {
        return RowCount;
    }

    public int GetColumnCount()
    {
        return ColumnCount;
    }

    public static List<List<Grid>> GetGridMatrix()
    {
        return GridMatrix;
    }

    public static void SetGridMatrix(List<List<Grid>> matrix)
    {
        GridMatrix = matrix;
    }

    void Start()
    {
        CreateGridMatrix();
        
       
    }


    void CreateGridMatrix()
    {
        if (GridMatrix.Count > 0)
        {
            GridMatrix.Clear();
        }
        Vector2 grid_start_pos = new Vector2(-RowCount, -ColumnCount);
        for(int i = 0; i < RowCount;i++)
        {
            List<Grid> grid_list = new List<Grid>();
            for(int j = 0; j < ColumnCount; j++)
            {

                GameObject grid_object = Instantiate(GridPrefab, Vector2.zero, Quaternion.identity, GridMatrixParent);
                Grid grid = grid_object.GetComponent<Grid>();
                GameObject game_block_object = GetRandomColorBlockFromPool();
                GameBlock game_block = game_block_object.GetComponent<GameBlock>();
                
                SetGridConditions(grid, i, j, TotalGridCount, grid_start_pos);
                AssignGameBlockToGrid(grid, game_block);

                grid_list.Add(grid);

                TotalGridCount++;

            }

            GridMatrix.Add(grid_list);
        }
        ScaleGridMatrix();
        SendGridMatrixCreatedMessage();
    }


    private void SendGridMatrixCreatedMessage()
    {
        EventParam grid_matrix_param = new EventParam();
        grid_matrix_param.SetGridMatrix(GridMatrix);
        EventManager.TriggerEvent(GameConstants.GameEvents.GRID_MATRIX_CREATED, grid_matrix_param);
    }

    GameObject GetRandomColorBlockFromPool()
    {
        ColorBlock.ColorBlockTypes[] LevelSpecificColorBlocks = GamePlayManager.GetLevelSpecificColorBlocks();
        int random_block_index = Random.Range(0, LevelSpecificColorBlocks.Length);
        GameObject color_block = PoolingManager.GetColorBlockFromPool(LevelSpecificColorBlocks[random_block_index]);
        return color_block;
    }


    void ScaleGridMatrix()
    {
        int diffirence = RowCount - ColumnCount;
        if (diffirence > 0)
        {
            SetGridSizeScaler(RowCount);
            GridMatrixParent.transform.localScale = Vector2.one / (GridSizeScaler);
        }

        else
        {
            SetGridSizeScaler(ColumnCount);
            GridMatrixParent.transform.localScale = Vector2.one / (GridSizeScaler);
        }
    }

    void SetGridSizeScaler(int matrix_count)
    {
        if (matrix_count <=5)
        {
            GridSizeScaler = 3;
        }
        if (matrix_count > 5 && matrix_count <= 9)
        {
            GridSizeScaler = 4;
        }
        if(matrix_count>9 && matrix_count <= 12)
        {
            GridSizeScaler = 5;
        }
        if(matrix_count>12 && matrix_count <= 16)
        {
            GridSizeScaler = 7;
        }

       
    }


    void AssignGameBlockToGrid(Grid grid, GameBlock game_block)
    {
        game_block.SetSpriteSortingOrder(TotalGridCount);
        game_block.transform.parent = grid.transform;
        game_block.transform.localScale = Vector3.one;
        game_block.transform.localPosition = Vector2.zero;
        game_block.gameObject.SetActive(true);
        grid.SetGridBlock(game_block);

    }

    void SetGridConditions(Grid grid, int matrix_index_x,int matrix_index_y, int sprite_order,Vector2 grid_start_pos)
    {

        MatrixPoint matrix_point = new MatrixPoint(matrix_index_x, matrix_index_y);
        grid.SetMatrixPoint(matrix_point);
        grid.SetSpriteOrderLevel(TotalGridCount);
        
        grid.gameObject.name = "Grid" + grid.GetMatrixPoint().GetMatrixIndexX() + "," + grid.GetMatrixPoint().GetMatrixIndexY();
        grid.transform.localPosition = grid_start_pos + new Vector2(GridPositionIndicator * matrix_index_x, GridPositionIndicator * matrix_index_y);

    }
}
