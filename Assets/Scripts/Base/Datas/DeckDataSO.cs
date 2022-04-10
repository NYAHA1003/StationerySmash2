using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeckData
{
    public List<CardData> cardDatas = new List<CardData>();
    
    public void Add_CardData(CardData data)
    {
        data.strategyData.Set_State();
        cardDatas.Add(data);
    }
}
