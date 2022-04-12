using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeckDataSO", menuName = "Scriptable Object/DeckDataSO")]
public class CardDeckSO : ScriptableObject
{
    public List<CardData> cardDatas = new List<CardData>();
}
   