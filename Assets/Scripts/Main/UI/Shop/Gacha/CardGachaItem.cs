using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CardGachaItem : GachaCard
{
    [SerializeField]
    private GameObject _sliceImage;
    public override void ActiveAndAnimate()
    {
        gameObject.SetActive(true);
    }
}
