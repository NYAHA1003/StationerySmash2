using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;

[System.Serializable]
public class DeckData
{
    public List<CardData> cardDatas = new List<CardData>();
    
    public void Add_CardData(CardData data)
    {
        StrategyData strategyData = StrategyDataManager.FindStrategyData(data.starategyType);
        strategyData.Set_State();
        cardDatas.Add(data);
    }
}
