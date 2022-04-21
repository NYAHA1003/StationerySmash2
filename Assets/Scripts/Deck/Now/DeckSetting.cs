using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class DeckSetting : MonoBehaviour
{
    [SerializeField]
    private UserDeckData userDeckData;
    [SerializeField]
    private GameObject cardPrefab;
    [SerializeField]
    private GameObject cardDescription;
    [SerializeField]
    private GameObject deckScroll; 
    public List<GameObject> deckCards = new List<GameObject>();
    private void Awake()
    {
        SetDeck();
    }
    private void Start()
    {
        EventManager.StartListening(EventsType.ActiveDeck, UpdateDeck);
        EventManager.StartListening(EventsType.ActiveCardInfoPn, OnActiveCardInfoPn);
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
            GameObject cardObj = CreateCard();
            cardObj.GetComponent<DeckCard>().SetCard(userDeckData.deckList.cardDatas[i]);
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
            deckCards[i].GetComponent<DeckCard>().SetCard(userDeckData.deckList.cardDatas[i]);
        }
    }
    /// <summary>
    /// ���� ī�� ���� 
    /// </summary>
    /// <returns></returns>
    public GameObject CreateCard()
    {
        GameObject cardObj = Instantiate(cardPrefab, deckScroll.transform.GetChild(0).GetChild(0),false);
        return cardObj;
    }

    /// <summary>
    /// ī�� ���� �г� Ȱ��ȭ
    /// </summary>
    private void OnActiveCardInfoPn()
    {
        cardDescription.SetActive(!cardDescription.activeSelf);
    }
}
