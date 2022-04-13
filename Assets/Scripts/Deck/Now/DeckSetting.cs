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
    /// �÷��̾� ���� ī�� ���� 
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
    /// ���� ������ �� ������Ʈ (ī�� �߰�)
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
    /// ���� ī�� ���� 
    /// </summary>
    /// <returns></returns>
    public DeckCard CreateCard()
    {
        DeckCard cardObj = Instantiate(cardPrefab);
        cardObj.transform.SetParent(gameObject.transform.GetChild(0).GetChild(0), false);
        return cardObj;
    }
}
