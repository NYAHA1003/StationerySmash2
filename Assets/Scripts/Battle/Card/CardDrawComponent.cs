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
        //�⺻ ����
        private int _maxCardCount = 3;
        private int _currentCardCount = 0;
        private bool _isDontUse = false;
        private int _cardIdCount = 0;
        private float _cardDelay = 0.0f;

        //���� ����
        private DeckData _deckData = new DeckData();
        private List<CardMove> _cardList = new List<CardMove>();
        private GameObject _cardMovePrefeb = null;
        private Transform _cardPoolManager = null;
        private Transform _cardCanvas = null;
        private RectTransform _cardSpawnPosition = null;
        
        /// <summary>
        /// �ʱ�ȭ
        /// </summary>
        /// <param name="deckData"></param>
        /// <param name="cardList"></param>
        /// <param name="cardPrefeb"></param>
        /// <param name="cardPool"></param>
        /// <param name="cardCanvas"></param>
        /// <param name="cardSpawnTrm"></param>
        public void SetInitialization(DeckData deckData, List<CardMove> cardList, GameObject cardPrefeb, Transform cardPool, Transform cardCanvas, RectTransform cardSpawnTrm)
		{
            _deckData = deckData;
            _cardList = cardList;
            _cardMovePrefeb = cardPrefeb;
            _cardPoolManager = cardPool;
            _cardCanvas = cardCanvas;
            _cardSpawnPosition = cardSpawnTrm;
		}

        /// <summary>
        /// ī�� ������ �̴´�
        /// </summary>
        public void AddOneCard()
        {
            //ī�带 ����� �� ����
            if (_isDontUse)
            {
                return;
            }

            //ī�尡 ������ ���� �ʴ´�
            if (_deckData.cardDatas.Count == 0)
            {
                return;
            }

            //ī�� �����͸� �������� ������
            int random = Random.Range(0, _deckData.cardDatas.Count);
            _currentCardCount++;

            //ī�带 Ǯ���ؼ� ������
            CardMove cardmove = PoolCard();
            cardmove.Set_UnitData(_deckData.cardDatas[random], _cardIdCount++);

            //ī�� ����Ʈ�� ī�带 ������
            _cardList.Add(cardmove);
        }

        /// <summary>
        /// ī�带 Ǯ����
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
        /// ī�带 ���� �� �ִ��� üũ�Ѵ�
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

        /// <summary>
        /// ���� ī�� ������ �ִ� ��������
        /// </summary>
        /// <returns></returns>
        public bool CheckMaxCard()
        {
            if (_currentCardCount >= _maxCardCount)
            {
                return true;
            }
            else
			{
                return false;
			}
        }

        /// <summary>
        /// ���� ī�� ������ ��ȯ
        /// </summary>
        /// <returns></returns>
        public int ReturnCurrentCard()
		{
            return _currentCardCount;
		}

        /// <summary>
        /// ���� ī�� ������ �����Ѵ�
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public void IncreaseCurrentCard(int num)
		{
            _currentCardCount += num;
		}

        /// <summary>
        /// �ִ� ī�� ����
        /// </summary>
        /// <param name="max">�ִ� ��</param>
        public void SetMaxCard(int max)
        {
            _maxCardCount = max;
        }

        /// <summary>
        /// �ִ� ī�� ����
        /// </summary>
        public void IncreaseMaxCard(int num)
        {
            _maxCardCount += num;
        }
    }
}