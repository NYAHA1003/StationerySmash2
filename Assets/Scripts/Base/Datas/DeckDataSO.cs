using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeckData
{
    public List<DataBase> cardDatas = new List<DataBase>();
    
    public void Add_CardData(DataBase data)
    {
        cardDatas.Add(data);
    }
}