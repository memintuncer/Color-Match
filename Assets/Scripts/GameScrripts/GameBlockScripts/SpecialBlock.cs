
using UnityEngine;

public class SpecialBlock : GameBlock
{
    public enum SpecialBlockType
    {
        None,
        Destroyer,
        Affected

         
        
    }

    [SerializeField] protected SpecialBlockType SpecialType;

    public SpecialBlockType GetSpecialType()
    {
        return SpecialType;
    }

}
