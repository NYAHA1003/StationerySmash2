using UnityEngine;
using Utill.Data;
using Utill.Tool;
using UnityEngine.UI;
using System.Collections.Generic;
using Main.Deck;
using Main.Event;
using Main.Buttons;

namespace Main.Buttons
{
    public class ButtonManager : MonoBehaviour
    {
        private Btn_MainPanel2 btn_MainPanel;

        [SerializeField]
        private Btn_MainPanel2 btn_MainPanel2;

        [Header("버튼")]
        [SerializeField]
        private List<Button> cardInfoBtns = new List<Button>();

        [SerializeField]
        private DeckSettingComponent deckSetting;
        private void Start()
        {
            btn_MainPanel2.Start();
            AddDeckCards();
            AddButtonEvent();
        }
        private void AddButtonEvent()
        {
            for (int i = 0; i < cardInfoBtns.Count; i++)
            {
                //참조 오류 방지용 캐싱
                int index = i;

                //카드 정보창을 여는 함수 호출
                cardInfoBtns[index].onClick.AddListener(
               () =>
               {
                   OnDeckDescriptoinActive(cardInfoBtns[index].GetComponent<DeckCard>());
               });
            }
        }

        /// <summary>
        /// 덱에 있는 카드의 버튼 컴포넌트를 리스트에 넣어두기 
        /// </summary>
        private void AddDeckCards()
        {
            for (int i = 0; i < deckSetting.deckCards.Count; i++)
            {
                cardInfoBtns.Add(deckSetting.deckCards[i].GetComponent<Button>());
            }
        }

        public void OnDeckDescriptoinActive(DeckCard deckCard)
        {
            EventManager.TriggerEvent(EventsType.ActiveCardDescription, deckCard);
        }

        /// <summary>
        /// 인자값번째 상점 패널 이동
        /// </summary>
        /// <param name="iParam"></param>
        public void OnMoveShopPanel(int iParam)
        {
            EventManager.TriggerEvent(EventsType.MoveShopPn, iParam);
        }
        /// <summary>
        /// 인자값번째 메인 패널 이동 
        /// </summary>
        /// <param name="iParam"></param>
        public void OnMoveMainPanel(int iParam)
        {
            EventManager.TriggerEvent(EventsType.MoveMainPn, iParam);
            EventManager.TriggerEvent(EventsType.CloaseAllPn);
        }

    }
}