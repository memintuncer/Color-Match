using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{

    [SerializeField] private  PoolingObject[] PoolObjectsCounts;
    [SerializeField] private static Transform PoolingParent;
    private static Dictionary<ColorBlock.ColorBlockTypes, List<GameObject>> ColorBlockPoolDictionary = new Dictionary<ColorBlock.ColorBlockTypes, List<GameObject>>();
    private static Dictionary<SpecialBlock.SpecialBlockType, List<GameObject>> SpecialBlockPoolDictionary = new Dictionary<SpecialBlock.SpecialBlockType, List<GameObject>>();



    private void OnEnable()
    {
        EventManager.StartListening(GameConstants.GameEvents.GAME_STARTED, CreatePool);
    }

    private void OnDisable()
    {
        EventManager.StopListening(GameConstants.GameEvents.GAME_STARTED, CreatePool);
    }


    private void CreatePool(EventParam param)
    {
        if (PoolingParent == null)
            PoolingParent = GameObject.FindWithTag("PoolingParent").transform;
        CreatePool();
    }

   

    
 

    public void CreatePool()
    {
        int count = 0;
        for(int i = 0; i < PoolObjectsCounts.Length; i++)
        {
            PoolingObject pool_object = PoolObjectsCounts[i];
            int object_count = pool_object.GetObjectCount();
            for(int j = 0; j < object_count; j++)
            {
                GameObject new_pool_object = Instantiate(pool_object.GetObjectPrefab(), Vector2.zero, Quaternion.identity,PoolingParent);
                
                new_pool_object.SetActive(false);
                new_pool_object.name = new_pool_object.name + count;
                SetPoolDictionary(new_pool_object);
                count++;
            }
            
        }
    }



    private void SetPoolDictionary(GameObject pool_object)
    {
        GameBlock game_block = pool_object.GetComponent<GameBlock>();

        switch (game_block.GetBlockType())
        {
            case GameBlock.BlockType.ColorBlock:
                SetColorBlockPoolDictionary((ColorBlock)game_block);
                break;

            case GameBlock.BlockType.Special:
                SetSpecialBlockPoolDictionary((SpecialBlock) game_block);
                break;
        }
        

    }

    private void SetColorBlockPoolDictionary(ColorBlock color_block)
    {
        ColorBlock.ColorBlockTypes block_color =  color_block.GetColorType();
        GameObject color_block_object = color_block.gameObject;

        if (ColorBlockPoolDictionary.ContainsKey(block_color))
        {
            ColorBlockPoolDictionary[block_color].Add(color_block_object);
        }

        else
        {
            List<GameObject> pool_object_list = new List<GameObject>();
            pool_object_list.Add(color_block_object);
            ColorBlockPoolDictionary.Add(block_color, pool_object_list);
        }
    }

    private void SetSpecialBlockPoolDictionary(SpecialBlock special_block)
    {
        //Create dictionary for special gameblocks

    }


    public static GameObject GetColorBlockFromPool(ColorBlock.ColorBlockTypes color_block_type)
    {
        List<GameObject> color_block_pool = ColorBlockPoolDictionary[color_block_type];
        GameObject color_block = color_block_pool[0];
        color_block_pool.RemoveAt(0);

        return color_block;
    }

    public  static void SendColorBlockBackToPool(ColorBlock color_block)
    {
        color_block.ResetGameBlock();
        color_block.transform.parent = PoolingParent;
        ColorBlock.ColorBlockTypes color_type = color_block.GetColorType();
        ColorBlockPoolDictionary[color_type].Add(color_block.gameObject);
        color_block.gameObject.SetActive(false);
       

    }


}
