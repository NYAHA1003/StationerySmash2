using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill; 

[CreateAssetMenu(fileName = "ShopItem", menuName = "Scriptable Object/ShopItemDataSO")]
public class ShopItemSO : MonoBehaviour
{
    public UnitType cardType;
    public string card_Name;
    public string card_Description;
    public int card_price;
    public int count; 

}
