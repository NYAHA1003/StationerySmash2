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

    private void Start()
    {
     //   EventManager.StartListening(EventsType.ActiveDeck, SetDeck);
    }
    [ContextMenu("SetDeck")]
    public void SetDeck()
    {
        userDeckData.SetCardData();
        for(int i = 0; i<  userDeckData.deckList.cardDatas.Count;  i++)
        {
            DeckCard cardObj = Instantiate(cardPrefab);
            cardObj.transform.SetParent(gameObject.transform.GetChild(0).GetChild(0),false); 
            cardObj.SetCard(userDeckData.deckList.cardDatas[i]);
        }
    }
}
