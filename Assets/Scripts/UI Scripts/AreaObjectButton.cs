using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AreaObjectButton : MonoBehaviour
{
    [SerializeField] private GameObject AreaObject;
    [SerializeField] private int RequiredStar;
    [SerializeField] private TextMeshProUGUI ReqStarText;


    public void CheckAreaObjectCanSpawn()
    {
        EventParam param = new EventParam();
        param.AreaObject = AreaObject;
        param.AreaObjectRequiredStar = RequiredStar;
        param.AreaObjectButton = transform.parent.gameObject;
        EventManager.TriggerEvent(GameConstants.GameEvents.SPAWN_AREA_OBJECT, param);
    }

    private void Start()
    {
        ReqStarText.text = RequiredStar.ToString();
    }
}
