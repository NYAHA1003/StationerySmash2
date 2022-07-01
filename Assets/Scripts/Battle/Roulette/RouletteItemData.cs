using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

[Serializable]
public class RouletteItemData 
{
    public Sprite _itemImage; // �귿 ������ �̹���
    public int _itemCount;  // �귿 ������ ���� 
    public RouletteItemType rulletItemType;

    [Range(1, 100)]
    public int _chance = 100; // Ȯ�� 

    public int index; // ������ ���� 
    public int weght; // ������ ����ġ 


}
