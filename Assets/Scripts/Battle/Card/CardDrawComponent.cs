using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Utill.Data;
using Utill.Tool;
using Main.Deck;

namespace Battle
{
	public class CardDrawComponent : BattleComponent
	{
        //기본 변수
        private int _maxCardCount = 3;
        private int _currentCardCount = 0;
        private bool _isDontUse = false;
        private int _cardIdCount = 0;
        private float _cardDelay = 0.0f;

        //참조 변수
        private DeckData _deckData = new DeckData();
        private List<CardMove> _cardList = new List<CardMove>();
        private GameObject _cardMovePrefeb = null;
        private Transform _cardPoolManager = null;
        private Transform _cardCanvas = null;
        private RectTransform _cardSpawnPosition = null;

        /// <summary>
        /// 카드 한장을 뽑는다
        /// </summary>
        public void AddOneCard()
        {
            //카드를 사용할 수 없다
            if (_isDontUse)
            {
                return;
            }

            //카드가 없으면 뽑지 않는다
            if (_deckData.cardDatas.Count == 0)
            {
                return;
            }

            //카드 데이터를 랜덤으로 선택함
            int random = Random.Range(0, _deckData.cardDatas.Count);
            _currentCardCount++;

            //카드를 풀링해서 가져옴
            CardMove cardmove = PoolCard();
            cardmove.Set_UnitData(_deckData.cardDatas[random], _cardIdCount++);

            //카드 리스트에 카드를 전달함
            _cardList.Add(cardmove);
        }

        /// <summary>
        /// 카드를 풀링함
        /// </summary>
        private CardMove PoolCard()
        {
            CardMove cardmove_obj = null;
            if (_cardPoolManager.childCount > 0)
            {
                cardmove_obj = _cardPoolManager.GetChild(0).gameObject.GetComponent<CardMove>();
                cardmove_obj.transform.position = _cardSpawnPosition.position;
                cardmove_obj.gameObject.SetActive(true);
            }
            cardmove_obj ??= PoolManager.CreateObject(_cardMovePrefeb, _cardSpawnPosition.position, Quaternion.identity).GetComponent<CardMove>();
            cardmove_obj.transform.SetParent(_cardCanvas);
            cardmove_obj.SetIsFusion(false);
            return cardmove_obj;
        }

        /// <summary>
        /// 카드를 뽑을 수 있는지 체크한다
        /// </summary>
        public bool CheckCardDraw()
        {
            if (_currentCardCount >= _maxCardCount)
            {
                return false;
            }
            if (_cardDelay > 0)
            {
                _cardDelay -= Time.deltaTime;
                return false;
            }
            _cardDelay = 0.3f;
            return true;
        }
    }
}