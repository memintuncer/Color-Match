using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private MatrixPoint MatrixPoint;
    private int SpriteOrderLevel;
    public GameBlock GridBlock;
    public bool IsEmpty;
    
    public MatrixPoint GetMatrixPoint()
    {
        return MatrixPoint;
    }

    public void SetMatrixPoint(MatrixPoint matrix_point)
    {
        MatrixPoint = matrix_point;
    }

    public int GetSpriteOrderLevel()
    {
        return SpriteOrderLevel;
    }

    public void SetSpriteOrderLevel(int sprite_order_level)
    {
        SpriteOrderLevel = sprite_order_level;
    }

    public GameBlock GetGridBlock()
    {
        return GridBlock;
    }

    public bool GetIsEmpty()
    {
        return IsEmpty;
    }
    
    public void SetGridBlock(GameBlock game_block)
    {
        IsEmpty = false;
        GridBlock = game_block;
        game_block.SetParentGrid(this);
        
    }

    protected void OnMouseDown()
    {
        GameBlock.BlockType block_type = GridBlock.GetBlockType();
        CheckSelectedGridBlock(block_type);


    }

    public void Deneme()
    {
        GameBlock.BlockType block_type = GridBlock.GetBlockType();
        CheckSelectedGridBlock(block_type);
    }

    private void CheckSelectedGridBlock(GameBlock.BlockType block_type)
    {
        switch (block_type)
        {
            case GameBlock.BlockType.ColorBlock:
                ColorBlock color_block = (ColorBlock)GridBlock;
                SendColorBlockSelectedMessage(color_block);
                break;

            case GameBlock.BlockType.Special:

                SpecialBlock special_block = (SpecialBlock)GridBlock;
                SpecialBlock.SpecialBlockType special_block_type = ((SpecialBlock)GridBlock).GetSpecialType();
                switch (special_block_type)
                {
                    case SpecialBlock.SpecialBlockType.Destroyer:
                        break;
                    case SpecialBlock.SpecialBlockType.Affected:
                        break;
                }
                break;
        }
    }


    private void SendColorBlockSelectedMessage(ColorBlock color_block)
    {
        
        EventParam selected_cube_param = new EventParam(color_block);
        EventManager.TriggerEvent(GameConstants.GameEvents.COLOR_BLOCK_SELECTED, selected_cube_param);
    }
    

    public void RemoveGameBlockFromGrid()
    {
        GridBlock = null;
        IsEmpty = true;
    }
}
