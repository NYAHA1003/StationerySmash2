using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

[Serializable]
public class RouletteItemData 
{
    public Sprite _itemImage; // 룰렛 아이템 이미지
    public int _itemCount;  // 룰렛 아이템 개수 
    public RouletteItemType rulletItemType;

    [Range(1, 100)]
    public int _chance = 100; // 확률 

    public int index; // 아이템 순번 
    public int weght; // 아이템 가중치 


}
