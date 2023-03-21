using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayManager : MonoBehaviour
{

    
    [SerializeField] private ColorBlock.ColorBlockTypes[] LevelSpecificColorBlocks;
    [SerializeField] Transform GameGoalsUI;
    [SerializeField] private int MoveCount;
    private Dictionary<ColorBlock.ColorBlockTypes, GameGoalUI> ColorBlockGameGoals = new Dictionary<ColorBlock.ColorBlockTypes, GameGoalUI>();
    private Dictionary<SpecialBlock.SpecialBlockType, GameGoalUI> SpecialGameBlockGameGoals = new Dictionary<SpecialBlock.SpecialBlockType, GameGoalUI>();
    
    [SerializeField] private TextMeshProUGUI MoveCountText;

    private int CompletedGoalCount=0, TotalGoalCount;
    [SerializeField] GameObject LevelSuccessUI, LevelFailUI;
    private bool LevelFinished = false;
    [SerializeField] RemoveMatchesAlgorithm RemoveMatchesAlgorithm;

    public ColorBlock.ColorBlockTypes[] GetLevelSpecificColorBlocks()
    {
        return LevelSpecificColorBlocks;
    }

    private List<List<ColorBlock>> MatchedColorBlocks = new List<List<ColorBlock>>();

    private void OnEnable()
    {
        EventManager.StartListening(GameConstants.GridMatrixEvents.SEARCH_COMPLETED, ChangeColorBlockSprites);
        EventManager.StartListening(GameConstants.GameEvents.COLOR_BLOCK_SELECTED, GetMatchedColorBlocks);
        EventManager.StartListening(GameConstants.GameEvents.CHECK_FOR_GAME_GOAL, CheckForGameGoal);
    }

    private void OnDisable()
    {
        EventManager.StopListening(GameConstants.GridMatrixEvents.SEARCH_COMPLETED, ChangeColorBlockSprites);
        EventManager.StopListening(GameConstants.GameEvents.COLOR_BLOCK_SELECTED, GetMatchedColorBlocks);
        EventManager.StopListening(GameConstants.GameEvents.CHECK_FOR_GAME_GOAL, CheckForGameGoal);
    }


    private void CheckForGameGoal(EventParam param)
    {
        
        List<ColorBlock> removed_color_blocks = param.GetRemovedColorBlocks();
        List<SpecialBlock> removed_special_blocks = param.GetRemovedSpecialBlocks();

        ColorBlock color_block = removed_color_blocks[0];
        ColorBlock.ColorBlockTypes color_type = color_block.GetColorType();

        

        if (ColorBlockGameGoals.ContainsKey(color_type))
        {
            GameGoalUI game_goal= ColorBlockGameGoals[color_type];
            int goal_count = game_goal.GetRequiredCount();
            goal_count -= removed_color_blocks.Count;
            if(goal_count <= 0)
            {
                ColorBlockGameGoals.Remove(color_type);
                goal_count = 0;
                CompletedGoalCount++;
                if(CompletedGoalCount == TotalGoalCount)
                {
                   
                    LevelFinished = true;
                    
                    StartCoroutine(LevelSuccessCRT());
                }
                
            }
            game_goal.SetRequiredCount(goal_count);


        }
        
        MoveCount--;
        if (MoveCount == 0)
        {
            CheckLevelSuccessOrFail();
        }
        
        MoveCountText.text = MoveCount.ToString();
    }


    private void CheckLevelSuccessOrFail()
    {
        LevelFinished = true;
        if (CompletedGoalCount < TotalGoalCount)
        {
            LevelFailUI.SetActive(true);

        }
    }

    IEnumerator LevelSuccessCRT()
    {
        yield return new WaitForSeconds(.5f);
        LevelSuccessUI.SetActive(true);
        yield return new WaitForSeconds(1.5f);
       
        SendGameBlocksToPool();
        EventManager.TriggerEvent(GameConstants.LEVEL_EVENTS.LEVEL_SUCCESSED, new EventParam());
    }

    void SendGameBlocksToPool()
    {
        List<List<Grid>> grid_matrix = GridMatrixManager.GetGridMatrix();
        int row = grid_matrix.Count;
        int col = grid_matrix[0].Count;

        for(int i = 0; i < row; i++)
        {
            for(int j = 0; j < col; j++)
            {
                GameBlock game_block = grid_matrix[i][j].GetGridBlock();

                switch (game_block.GetBlockType())
                {
                    case GameBlock.BlockType.ColorBlock:
                        ColorBlock color_block = (ColorBlock)game_block;
                        PoolingManager.SendColorBlockBackToPool(color_block);
                        break;
                    case GameBlock.BlockType.Special:
                        break;
                }

                
            }
        }

    }
    private void ChangeColorBlockSprites(EventParam param)
    {

        MatchedColorBlocks = param.GetMatchedColorBlocks();
        for(int i = 0; i < MatchedColorBlocks.Count; i++)
        {
            int count = MatchedColorBlocks[i].Count;
            int sprite_index = FindSpriteIndex(count);
  
            foreach(ColorBlock color_block in MatchedColorBlocks[i])
            {
                    
                color_block.SetBlockSprite(sprite_index);
            }
            
        }
    }


    private void GetMatchedColorBlocks(EventParam param)
    {
        if (!LevelFinished)
        {
            param.SetMatchedColorBlocks(MatchedColorBlocks);
            EventManager.TriggerEvent(GameConstants.GameEvents.REMOVE_MATCHED_COLOR_BLOCKS, param);
        }
        
       
    }

    public void RestartLevel()
    {
        StartCoroutine(RestartLevelCRT());
    }

    IEnumerator RestartLevelCRT()
    {
        yield return new WaitForSeconds(.5f);
        RemoveMatchesAlgorithm.gameObject.SetActive(false);
        SendGameBlocksToPool();
        EventManager.TriggerEvent(GameConstants.LEVEL_EVENTS.RESTART_LEVEL, new EventParam());
    }

    private int FindSpriteIndex(int count)
    {
        int sprite_index = 0;

        if (count <= 4)
        {
            sprite_index = 0;
        }
        if (count > 4 && count < 8)
        {
            sprite_index = 1;
        }

        if (count > 7 && count < 10)
        {
            sprite_index = 2;
        }

        if (count > 10)
        {
            sprite_index = 3;
        }

        return sprite_index;

    }

    void Start()
    {
        GetGameGoals();
    }

    void GetGameGoals()
    {
        int game_goal_count = GameGoalsUI.transform.childCount;
        TotalGoalCount = game_goal_count;
        for(int i = 0; i < game_goal_count; i++)
        {
            
            GameGoalUI game_goal_UI = GameGoalsUI.GetChild(i).GetComponent<GameGoalUI>();

            GameGoalTypes game_goal_type = game_goal_UI.GetGameGoalType();

            switch (game_goal_type)
            {
                case GameGoalTypes.ColorBlockGameGoal:
                    SetColorBlockGameGoalsDict(game_goal_UI);
                    break;
                case GameGoalTypes.SpecialGameBlockGoal:
                    SetSpecialGameBlockGameGoalsDict(game_goal_UI);
                    break;
            }
        }
        MoveCountText.text = MoveCount.ToString();

    }


    void SetColorBlockGameGoalsDict(GameGoalUI game_goal_ui)
    {
        ColorBlock.ColorBlockTypes color_type = game_goal_ui.GetColorBlockType();


        ColorBlockGameGoals.Add(color_type, game_goal_ui);
        
       
    }

    void SetSpecialGameBlockGameGoalsDict(GameGoalUI game_goal_ui)
    {
        SpecialBlock.SpecialBlockType special_type = game_goal_ui.GetSpecialBlockType();


        SpecialGameBlockGameGoals.Add(special_type, game_goal_ui);
    }
  
}
