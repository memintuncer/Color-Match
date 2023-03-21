using UnityEngine;

public class GameConstants
{
    
    public struct LEVEL_EVENTS
    {
       
        public static string OBJECTIVE_FAILED = "OBJECTIVE_FAILED";
        public static string LEVEL_FINISHED = "LEVEL_FINISHED";
        public static string LEVEL_FAILED = "LEVEL_FAILED";
        public static string LEVEL_SUCCESSED = "LEVEL_SUCCESSED";
        public static string LEVEL_STARTED = "LEVEL_STARTED";
        public static string RESTART_LEVEL = "RESTART_LEVEL";


    }

   
    public struct GameEvents
    {
       
        public static string GAME_STARTED = "GAME_STARTED";
        public static string LOAD_SPECIFIC_LEVEL = "LOAD_SPECIFIC_LEVEL";
        public static string GRID_MATRIX_CREATED = "GRID_MATRIX_CREATED";
        public static string GRID_MATRIX_UPDATED = "GRID_MATRIX_UPDATED";


        public static string COLOR_BLOCK_SELECTED = "COLOR_BLOCK_SELECTED";
        public static string DESTROYER_BLOCK_SELECTED = "DESTROYER_BLOCK_SELECTED";
        
        
        public static string REMOVE_MATCHED_COLOR_BLOCKS = "REMOVE_MATCHED_COLOR_BLOCKS";
        public static string MATCHED_REMOVED = "MATCHED_REMOVED";
        public static string CHECK_FOR_GAME_GOAL = "CHECK_FOR_GAME_GOAL";
        public static string GENEREATE_NEW_GAME_BLOCKS_FOR_EMPTY_GRIDS = "GENEREATE_NEW_GAME_BLOCKS_FOR_EMPTY_GRIDS";

        public static string SPAWN_AREA_OBJECT = "SPAWN_AREA_OBJECT";

    }


    public struct GridMatrixEvents
    {
        public static string SEARCH_COMPLETED = "SEARCH_COMPLETED";
        public static string GRID_MATRIX_UPDATED = "GRID_MATRIX_UPDATED";
        public static string SHUFFLE_MATRIX = "SHUFFLE_MATRIX";
    }
    

   
    

}