using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CardGachaItem : GachaCard
{
    [SerializeField]
    private TextMeshProUGUI _sliceCount;// ���� ���� 
    [SerializeField]
    private CardMesh _cardMesh; 
    public override void ActiveAndAnimate()
    {
        gameObject.SetActive(true);
    }
}
