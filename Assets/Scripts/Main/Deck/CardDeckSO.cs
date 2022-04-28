using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Deck
{

    [CreateAssetMenu(fileName = "DeckDataSO", menuName = "Scriptable Object/DeckDataSO")]
    public class CardDeckSO : ScriptableObject
    {
        //ī�� ������ ����
        public List<CardData> cardDatas = new List<CardData>();
    }
}