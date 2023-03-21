
using UnityEngine;

//Type of block that cannot be selected but is affected by the explosion of neighboring blocks

public class AffectedBlock
{

    public enum AffectedBlockType
    {
        None,
        Baloon,
        Box
    }

    [SerializeField] AffectedBlockType AffectedType;

    protected virtual void CheckExplosionCondition()
    {

    }
}
