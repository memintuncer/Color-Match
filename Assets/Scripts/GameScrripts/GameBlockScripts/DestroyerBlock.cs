using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerBlock : SpecialBlock
{
    public enum DestroyerBlockType
    {
        None,
        Bomb,
        Rocket,
        Star
    }

    [SerializeField] private DestroyerBlockType DestroyerType;

    public virtual void DestroyOtherGrids()
    {

    }
}
