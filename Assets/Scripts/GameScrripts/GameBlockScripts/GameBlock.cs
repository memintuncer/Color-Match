
using UnityEngine;

public abstract class GameBlock : MonoBehaviour
{

    public enum BlockType
    {
        None,
        ColorBlock,
        Special
    }

    [SerializeField] protected BlockType GameBlockType;

    [SerializeField] protected SpriteRenderer SpriteRenderer;
    protected Grid ParentGrid;

    public float speed;
    [SerializeField] protected bool CanMove = true;
    public bool Sliding;
    public void SetParentGrid(Grid parent_grid)
    {
        ParentGrid = parent_grid;
    }

    public Grid GetParentGrid()
    {
        return ParentGrid;
    }

    public BlockType GetBlockType()
    {
        return GameBlockType;
    }


    public bool GetCubeCanMove()
    {
        return CanMove;
    }


    
    void Update()
    {
        if (Sliding)
        {
            Slide();
        }
        
    }

    public virtual void Slide()
    {
        
        
        transform.localPosition =  Vector3.Lerp(transform.localPosition, Vector3.zero, speed * Time.deltaTime);
        if (transform.localPosition == Vector3.zero)
        {
            Sliding = false;
        }
        
    }

    public void SetSpriteSortingOrder(int new_sorting_order)
    {
        SpriteRenderer.sortingOrder = new_sorting_order;
    }


  
    public virtual void ResetGameBlock()
    {
        ParentGrid.RemoveGameBlockFromGrid();
        ParentGrid = null;
        transform.parent = null;
        
    }
}
