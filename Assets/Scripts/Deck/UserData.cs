using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill ; 
public class UserData
{
    private Dictionary<UnitType, CardData> userCards = new Dictionary<UnitType, CardData>();
    CardData GetData()
    {
        UnitType unitType = TempDataBase.Instance.ReturnDataType(); 
        if (!userCards.TryGetValue(TempDataBase.Instance.ReturnDataType(), out CardData cardData))
        {
            CardData tempCardData = TempDataBase.Instance.ReturnData(); 
            userCards.Add(unitType, tempCardData);
            cardData = tempCardData;
        }
        return cardData; 
    }

    void SetData()
    {

    }
}
