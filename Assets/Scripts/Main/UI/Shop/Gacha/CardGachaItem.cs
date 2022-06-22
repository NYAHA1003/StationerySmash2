using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CardGachaItem : GachaCard
{
    public override void ActiveAndAnimate()
    {
        gameObject.SetActive(true);
    }
}
