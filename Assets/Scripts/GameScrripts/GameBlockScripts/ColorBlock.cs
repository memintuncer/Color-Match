using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBlock : GameBlock
{

    public enum ColorBlockTypes
    {
        None,
        Blue,
        Green,
        Pink,
        Purple,
        Red,
        Yellow
    }

    [SerializeField] private Sprite[] BlockSprites;
    [SerializeField] private ColorBlockTypes ColorType;


    public ColorBlockTypes GetColorType()
    {
        return ColorType;
    }

    private void Start()
    {
        GetSpriteRenderer();
    }

    public void SetBlockSprite(int sprite_index)
    {
        SpriteRenderer.sprite = BlockSprites[sprite_index];
    }

   

    void GetSpriteRenderer()
    {
        SpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    public override void ResetGameBlock()
    {
        base.ResetGameBlock();
        
    }

   
}
