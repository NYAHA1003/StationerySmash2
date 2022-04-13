using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class DeckSetting : MonoBehaviour
{
    [SerializeField]
    private UserDeckData userDeckData;
    [SerializeField]
    private DeckCard cardPrefab;

    private List<DeckCard> deckCards = new List<DeckCard>();
    private void Start()
    {
        EventManager.StartListening(EventsType.ActiveDeck, UpdateDeck);
        SetDeck(); 
    }
    
    /// <summary>
    /// 플레이어 덱에 카드 세팅 
    /// </summary>
    [ContextMenu("SetDeck")]
    public void SetDeck()
    {
        userDeckData.SetCardData();
        for (int i = 0; i < userDeckData.deckList.cardDatas.Count; i++)
        {
            DeckCard cardObj = CreateCard();
            cardObj.SetCard(userDeckData.deckList.cardDatas[i]);
            deckCards.Add(cardObj);
        }
    }
    /// <summary>
    /// 게임 실행중 덱 업데이트 (카드 추가)
    /// </summary>
    public void UpdateDeck()
    {
        userDeckData.SetCardData();
        for (int i = 0; i < deckCards.Count; i++)
        {
            if (deckCards[i] == null) deckCards[i] = CreateCard();
            deckCards[i].SetCard(userDeckData.deckList.cardDatas[i]);
        }
    }
    /// <summary>
    /// 덱에 카드 생성 
    /// </summary>
    /// <returns></returns>
    public DeckCard CreateCard()
    {
        DeckCard cardObj = Instantiate(cardPrefab);
        cardObj.transform.SetParent(gameObject.transform.GetChild(0).GetChild(0), false);
        return cardObj;
    }
}
