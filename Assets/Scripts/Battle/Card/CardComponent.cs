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
    [System.Serializable]
    public class CardComponent : BattleComponent, IWinLose
    {
        //������Ƽ
        public List<CardObj> CardList => _cardList;

        //�Ӽ�
        public bool IsAlwaysSpawn => _isAlwaysSpawn;
        public bool IsDontUse => _isDontUse;

        //�⺻ ����
        private Coroutine _delayCoroutine = null;
        private bool _isDontUse = false;

        //�ν����� ���� ����
        [SerializeField]
        private GameObject _summonRangeImage = null;
        [SerializeField]
        private RectTransform _summonArrow = null;
        [SerializeField]
        private GameObject _cardMovePrefeb = null;
        [SerializeField]
        private Transform _cardPoolManager = null;
        [SerializeField]
        private Transform _cardCanvas = null;
        [SerializeField]
        private RectTransform _cardLeftPosition = null;
        [SerializeField]
        private RectTransform _cardRightPosition = null;
        [SerializeField]
        private RectTransform _cardSpawnPosition = null;
        [SerializeField]
        private GameObject _unitAfterImage = null;
        [SerializeField]
        private SpriteRenderer _afterImageSpriteRenderer = null;
        [SerializeField]
        private bool _isAlwaysSpawn = false;
        [SerializeField]
        private CardDeckSO _cardDeckSO = null;

        //���� ����
        private StageData _stageData = null;
        private DeckData _deckData = new DeckData();
        private List<CardObj> _cardList = new List<CardObj>();
        private UnitComponent _commandUnit = null;
        private CostComponent _commandCost = null;
        private WinLoseComponent _commandWinLose = null;
        private CameraComponent _commandCamera = null;
        private MonoBehaviour _managerBase = null;
        private CardDrowComponent _cardDrawComponent = null;
        private CardRangeComponent _cardRangeComponent = null;
        private CardSelectComponent _cardSelectComponent = null;
        private CardFusionComponent _cardFusionComponent = null;
        private CardSortComponent _cardSortComponent = null;

        /// <summary>
        /// �ʱ�ȭ
        /// </summary>
        public void SetInitialization(MonoBehaviour managerBase, WinLoseComponent commandWinLose, CameraComponent commandCamera, UnitComponent commandUnit, CostComponent commandCost, ref System.Action updateAction, StageData stageData, int maxCard)
        {
            _cardDrawComponent = new CardDrowComponent();
            _cardRangeComponent = new CardRangeComponent();
            _cardSelectComponent = new CardSelectComponent();
            _cardFusionComponent = new CardFusionComponent();
            _cardSortComponent = new CardSortComponent();

            //������ ����
            this._managerBase = managerBase;
            this._stageData = stageData;
            this._commandUnit = commandUnit;
            this._commandCost = commandCost;
            this._commandCamera = commandCamera;
            this._commandWinLose = commandWinLose;

            //������ ����Ѵ�
            this._commandWinLose.AddObservers(this);

            SetMaxCard(maxCard);

            //���� ī�������� ����
            SetDeckCard();

            _cardDrawComponent.SetInitialization(this, _deckData, _cardList, _cardMovePrefeb, _cardPoolManager, _cardCanvas, _cardSpawnPosition);
            _cardRangeComponent.SetInitialization(this, _cardSelectComponent, _commandCost, _summonRangeImage, _summonArrow, _stageData, _unitAfterImage, _afterImageSpriteRenderer);
            _cardSelectComponent.SetInitialization(this, _commandUnit, _commandCost);
            _cardFusionComponent.SetInitialization(this, _managerBase);
            _cardSortComponent.SetInitialization(this, _cardSelectComponent, _cardLeftPosition, _cardRightPosition);

            //������Ʈ�� �Լ��� ����
            updateAction += UpdateUnitAfterImage;
            updateAction += UpdateSelectCardPos;
            updateAction += UpdateCardDraw;
            updateAction += UpdateSummonRange;
            updateAction += UpdateCheckCost;
        }

        /// <summary>
        /// �¸����� ����
        /// </summary>
        /// <param name="isWin"></param>
        public void Notify(bool isWin)
        {
            //ī�带 �� �̻� ����� �� ���� �Ѵ�
            _isDontUse = true;

            _cardSelectComponent.DontUseSelectedCard();
        }

        /// <summary>
        /// Ʃ�丮�� üũ
        /// </summary>
        public void CheckTutorialCard()
        {
            if(_cardDrawComponent.CheckMaxCard())
            {
                BattleTurtorialComponent.tutorialEventQueue.Dequeue().Invoke(); 
            }
        }

        /// <summary>
        /// ī�� �� ���� �̴´�
        /// </summary>
        public void AddOneCard()
		{
            //ī�� �̱�
            _cardDrawComponent.AddOneCard();

            //ī�� ���� �� ���� ȿ���� ����
            RunAction(AddOneCard);
        }

        /// <summary>
        /// ��� ī�带 �����
        /// </summary>
        public void ClearCards()
        {
            //ī�� ���� ���
            if (_delayCoroutine != null)
            {
                _managerBase.StopCoroutine(_delayCoroutine);
            }
            //ī��� ��� ����
            for (; _cardDrawComponent.ReturnCurrentCard() > 0;)
            {
                SubtractLastCard();
            }
        }

        /// <summary>
        /// �ִ� ī�� ����
        /// </summary>
        /// <param name="max">�ִ� ��</param>
        public void SetMaxCard(int max)
        {
            _cardDrawComponent.SetMaxCard(max);
        }

        /// <summary>
        /// �ִ� ī�� ����
        /// </summary>
        public void IncreaseMaxCard(int num)
        {
            _cardDrawComponent.IncreaseMaxCard(num);
        }
        //ī�� ���� ����
        /// <summary>
        /// ī�带 ������
        /// </summary>
        /// <param name="card"></param>
        public void SelectCard(CardObj card)
        {
            //ī�带 ����� �� ����
            if (_isDontUse)
            {
                return;
            }

            //�ش� ī�带 ������
            _cardSelectComponent.SelectCard(card);
        }

        /// <summary>
        /// ī�� ������ �����
        /// </summary>
        /// <param name="card"></param>
        public void SetUnSelectCard(CardObj card)
        {
            _cardSelectComponent.SetUnSelectCard(card);
        }

        /// <summary>
        /// ī�带 ����Ѵ�
        /// </summary>
        /// <param name="card"></param>
        public void SetUseCard(CardObj card)
        {
            //��ȯ�� �� �ִ��� üũ �� ��ȯ ���� �׸��� ����
            SetSummonRangeLine(false);

            //ī�带 ����Ѵ�
            if(_cardSelectComponent.SetUseCard(card))
			{
                //ī�� ����
                SetDelayFusion();
			}

        }

        /// <summary>
        /// ���տ� �����̸� ����, �����ϴ� �Լ�
        /// </summary>
        public void SetDelayFusion()
        {
            _cardFusionComponent.SetFusionAndDelay();
        }

        /// <summary>
        /// ī�带 ã�Ƽ� ����Ʈ���� �����Ѵ�
        /// </summary>
        /// <param name="cardMove"></param>
        public void SubtractCardFind(CardObj cardMove)
        {
            SubtractCardAt(_cardList.FindIndex(x => x.Id == cardMove.Id));
        }

        /// <summary>
        /// ������ �ε����� ī�带 �����
        /// </summary>
        public void SubtractCardAt(int index)
        {
            if (_cardDrawComponent.ReturnCurrentCard() == 0)
            {
                return;
            }

            //ī�� ����
            _cardDrawComponent.IncreaseCurrentCard(-1);
            _cardList[index].transform.SetParent(_cardPoolManager);
            _cardList[index].gameObject.SetActive(false);
            _cardList.RemoveAt(index);

            //�����ϰ� ī�带 ����
            _cardSortComponent.SortCard();
        }

        /// <summary>
        /// ��ȯ ���� �׸��⸦ Ű�ų� ����
        /// </summary>
        /// <param name="isActive"></param>
        public void SetSummonRangeLine(bool isActive)
        {
            _cardRangeComponent.SetSummonRangeLine(isActive);
        }

        /// <summary>
        /// ī�� ��ȯ �ִ� ���� ��ȯ
        /// </summary>
        public float ReturnMaxRange()
		{
            return _cardRangeComponent.MaxSummonRange;
        }

        /// <summary>
        /// ī�� ��ȯ �ּ� ���� ��ȯ
        /// </summary>
        public float ReturnMinRange()
        {
            return _cardRangeComponent.MaxSummonRange;
        }

        /// <summary>
        /// ī�带 �����Ѵ�
        /// </summary>
        public void SortCard()
        {
            _cardSortComponent.SortCard();
        }

        /// <summary>
        /// ī�尡 ���õǾ����� ��ȯ�Ѵ�
        /// </summary>
        /// <returns></returns>
        public bool ReturnIsSelected()
		{
            return _cardSelectComponent.IsSelectCard;
		}

        /// <summary>
        /// ������ ī�带 �����
        /// </summary>
        private void SubtractLastCard()
        {
            SubtractCardAt(_cardDrawComponent.ReturnCurrentCard() - 1);
        }
        
        /// <summary>
        /// ������ ī�� ��ġ�� ������Ʈ �Ѵ�
        /// </summary>
        private void UpdateSelectCardPos()
        {
            _cardSelectComponent.UpdateSelectCardPos();
        }

        /// <summary>
        /// �ڵ� ī�� ��ο� ������Ʈ
        /// </summary>
        private void UpdateCardDraw()
        {
            if (_cardDrawComponent.CheckCardDraw())
            {
                AddOneCard();
            }
        }

        /// <summary>
        /// ���� ī�� �������� �ִ´�
        /// </summary>
        private void SetDeckCard()
        {
            int count = _cardDeckSO.cardDatas.Count;
            for (int i = 0; i < count; i++)
            {
                CardData cardData = _cardDeckSO.cardDatas[i];
                _deckData.Add_CardData(cardData);
            }
        }

        /// <summary>
        /// ī�� ��ȯ �̸�����
        /// </summary>
        /// <param name="unitData"></param>
        /// <param name="pos"></param>
        /// <param name="isDelete"></param>
        private void UpdateUnitAfterImage()
        {
            _cardRangeComponent.UpdateUnitAfterImage();
        }

        /// <summary>
        /// ��ȯ ���� ������Ʈ �� ����
        /// </summary>
        private void UpdateSummonRange()
        {
            _cardRangeComponent.UpdateSummonRange();
        }

        /// <summary>
        /// ī����� �ڽ�Ʈ�� ���Ͽ� ����� �� �ִ��� Ȯ���Ѵ�
        /// </summary>
        private void UpdateCheckCost()
        {
            int count = _cardList.Count;
            for (int i = 0; i < count; i++)
            {
                _cardList[i].CheckCost(_commandCost.CurrentCost);
            }
        }
    }

}