using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameGoalUI : MonoBehaviour
{
    [SerializeField] GameGoalTypes GameGoalType;
    [SerializeField] private int RequiredCount;
    private TextMeshProUGUI RequiredCountText;

    [SerializeField] private ColorBlock.ColorBlockTypes ColorBlockType;
    [SerializeField] private SpecialBlock.SpecialBlockType SpecialBlockType;

    public GameGoalTypes GetGameGoalType()
    {
        return GameGoalType;
    }

    public TextMeshProUGUI GetRequiredCountText()
    {
        return RequiredCountText;
    }

    public void SetRequiredCountText(int new_count)
    {
        RequiredCount = new_count;
        RequiredCountText.text = new_count.ToString();
    }

    public int GetRequiredCount()
    {
        return RequiredCount;
    }

    public void SetRequiredCount(int new_count)
    {
        RequiredCount = new_count;
        RequiredCountText.text = RequiredCount.ToString();
    }


    public ColorBlock.ColorBlockTypes GetColorBlockType()
    {
        return ColorBlockType;
    }

    public SpecialBlock.SpecialBlockType GetSpecialBlockType()
    {
        return SpecialBlockType;
    }

    private void Start()
    {
        RequiredCountText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        RequiredCountText.text = RequiredCount.ToString();
    }
}
