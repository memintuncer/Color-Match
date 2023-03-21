using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PoolingObject
{

    [SerializeField] private GameBlock.BlockType ObjectType;
    [SerializeField] private GameObject ObjectPrefab;
    [SerializeField] private int ObjectCount;
    

    public GameBlock.BlockType GetObjectType()
    {
        return ObjectType;
    }

    public int GetObjectCount()
    {
        return ObjectCount;
    }

    public GameObject GetObjectPrefab()
    {
        return ObjectPrefab;
    }


}
