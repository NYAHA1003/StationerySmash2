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
        //프로퍼티
        public List<CardObj> CardList => _cardList;

        //속성
        public bool IsAlwaysSpawn => _isAlwaysSpawn;
        public bool IsDontUse => _isDontUse;

        //기본 변수
        private Coroutine _delayCoroutine = null;
        private bool _isDontUse = false;

        //인스펙터 참조 변수
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

        //참조 변수
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
        /// 초기화
        /// </summary>
        public void SetInitialization(MonoBehaviour managerBase, WinLoseComponent commandWinLose, CameraComponent commandCamera, UnitComponent commandUnit, CostComponent commandCost, ref System.Action updateAction, StageData stageData, int maxCard)
        {
            _cardDrawComponent = new CardDrowComponent();
            _cardRangeComponent = new CardRangeComponent();
            _cardSelectComponent = new CardSelectComponent();
            _cardFusionComponent = new CardFusionComponent();
            _cardSortComponent = new CardSortComponent();

            //변수들 설정
            this._managerBase = managerBase;
            this._stageData = stageData;
            this._commandUnit = commandUnit;
            this._commandCost = commandCost;
            this._commandCamera = commandCamera;
            this._commandWinLose = commandWinLose;

            //관찰자 등록한다
            this._commandWinLose.AddObservers(this);

            SetMaxCard(maxCard);

            //덱에 카드정보들 전달
            SetDeckCard();

            _cardDrawComponent.SetInitialization(this, _deckData, _cardList, _cardMovePrefeb, _cardPoolManager, _cardCanvas, _cardSpawnPosition);
            _cardRangeComponent.SetInitialization(this, _cardSelectComponent, _commandCost, _summonRangeImage, _summonArrow, _stageData, _unitAfterImage, _afterImageSpriteRenderer);
            _cardSelectComponent.SetInitialization(this, _commandUnit, _commandCost);
            _cardFusionComponent.SetInitialization(this, _managerBase);
            _cardSortComponent.SetInitialization(this, _cardSelectComponent, _cardLeftPosition, _cardRightPosition);

            //업데이트할 함수들 전달
            updateAction += UpdateUnitAfterImage;
            updateAction += UpdateSelectCardPos;
            updateAction += UpdateCardDraw;
            updateAction += UpdateSummonRange;
            updateAction += UpdateCheckCost;
        }

        /// <summary>
        /// 승리관련 관찰
        /// </summary>
        /// <param name="isWin"></param>
        public void Notify(bool isWin)
        {
            //카드를 더 이상 사용할 수 없게 한다
            _isDontUse = true;

            _cardSelectComponent.DontUseSelectedCard();
        }

        /// <summary>
        /// 튜토리얼 체크
        /// </summary>
        public void CheckTutorialCard()
        {
            if(_cardDrawComponent.CheckMaxCard())
            {
                BattleTurtorialComponent.tutorialEventQueue.Dequeue().Invoke(); 
            }
        }

        /// <summary>
        /// 카드 한 장을 뽑는다
        /// </summary>
        public void AddOneCard()
		{
            //카드 뽑기
            _cardDrawComponent.AddOneCard();

            //카드 뽑을 때 연계 효과들 실행
            RunAction(AddOneCard);
        }

        /// <summary>
        /// 모든 카드를 지운다
        /// </summary>
        public void ClearCards()
        {
            //카드 융합 취소
            if (_delayCoroutine != null)
            {
                _managerBase.StopCoroutine(_delayCoroutine);
            }
            //카드들 모두 삭제
            for (; _cardDrawComponent.ReturnCurrentCard() > 0;)
            {
                SubtractLastCard();
            }
        }

        /// <summary>
        /// 최대 카드 설정
        /// </summary>
        /// <param name="max">최대 수</param>
        public void SetMaxCard(int max)
        {
            _cardDrawComponent.SetMaxCard(max);
        }

        /// <summary>
        /// 최대 카드 증감
        /// </summary>
        public void IncreaseMaxCard(int num)
        {
            _cardDrawComponent.IncreaseMaxCard(num);
        }
        //카드 선택 관련
        /// <summary>
        /// 카드를 선택함
        /// </summary>
        /// <param name="card"></param>
        public void SelectCard(CardObj card)
        {
            //카드를 사용할 수 없다
            if (_isDontUse)
            {
                return;
            }

            //해당 카드를 선택함
            _cardSelectComponent.SelectCard(card);
        }

        /// <summary>
        /// 카드 선택을 취소함
        /// </summary>
        /// <param name="card"></param>
        public void SetUnSelectCard(CardObj card)
        {
            _cardSelectComponent.SetUnSelectCard(card);
        }

        /// <summary>
        /// 카드를 사용한다
        /// </summary>
        /// <param name="card"></param>
        public void SetUseCard(CardObj card)
        {
            //소환할 수 있는지 체크 및 소환 범위 그리기 없앰
            SetSummonRangeLine(false);

            //카드를 사용한다
            if(_cardSelectComponent.SetUseCard(card))
			{
                //카드 융합
                SetDelayFusion();
			}

        }

        /// <summary>
        /// 융합에 딜레이를 설정, 리셋하는 함수
        /// </summary>
        public void SetDelayFusion()
        {
            _cardFusionComponent.SetFusionAndDelay();
        }

        /// <summary>
        /// 카드를 찾아서 리스트에서 제거한다
        /// </summary>
        /// <param name="cardMove"></param>
        public void SubtractCardFind(CardObj cardMove)
        {
            SubtractCardAt(_cardList.FindIndex(x => x.Id == cardMove.Id));
        }

        /// <summary>
        /// 지정한 인덱스의 카드를 지운다
        /// </summary>
        public void SubtractCardAt(int index)
        {
            if (_cardDrawComponent.ReturnCurrentCard() == 0)
            {
                return;
            }

            //카드 삭제
            _cardDrawComponent.IncreaseCurrentCard(-1);
            _cardList[index].transform.SetParent(_cardPoolManager);
            _cardList[index].gameObject.SetActive(false);
            _cardList.RemoveAt(index);

            //삭제하고 카드를 정렬
            _cardSortComponent.SortCard();
        }

        /// <summary>
        /// 소환 범위 그리기를 키거나 끄기
        /// </summary>
        /// <param name="isActive"></param>
        public void SetSummonRangeLine(bool isActive)
        {
            _cardRangeComponent.SetSummonRangeLine(isActive);
        }

        /// <summary>
        /// 카드 소환 최대 범위 반환
        /// </summary>
        public float ReturnMaxRange()
		{
            return _cardRangeComponent.MaxSummonRange;
        }

        /// <summary>
        /// 카드 소환 최소 범위 반환
        /// </summary>
        public float ReturnMinRange()
        {
            return _cardRangeComponent.MaxSummonRange;
        }

        /// <summary>
        /// 카드를 정렬한다
        /// </summary>
        public void SortCard()
        {
            _cardSortComponent.SortCard();
        }

        /// <summary>
        /// 카드가 선택되었는지 반환한다
        /// </summary>
        /// <returns></returns>
        public bool ReturnIsSelected()
		{
            return _cardSelectComponent.IsSelectCard;
		}

        /// <summary>
        /// 마지막 카드를 지운다
        /// </summary>
        private void SubtractLastCard()
        {
            SubtractCardAt(_cardDrawComponent.ReturnCurrentCard() - 1);
        }
        
        /// <summary>
        /// 선택한 카드 위치를 업데이트 한다
        /// </summary>
        private void UpdateSelectCardPos()
        {
            _cardSelectComponent.UpdateSelectCardPos();
        }

        /// <summary>
        /// 자동 카드 드로우 업데이트
        /// </summary>
        private void UpdateCardDraw()
        {
            if (_cardDrawComponent.CheckCardDraw())
            {
                AddOneCard();
            }
        }

        /// <summary>
        /// 덱에 카드 정보들을 넣는다
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
        /// 카드 소환 미리보기
        /// </summary>
        /// <param name="unitData"></param>
        /// <param name="pos"></param>
        /// <param name="isDelete"></param>
        private void UpdateUnitAfterImage()
        {
            _cardRangeComponent.UpdateUnitAfterImage();
        }

        /// <summary>
        /// 소환 범위 업데이트 및 증가
        /// </summary>
        private void UpdateSummonRange()
        {
            _cardRangeComponent.UpdateSummonRange();
        }

        /// <summary>
        /// 카드들의 코스트를 비교하여 사용할 수 있는지 확인한다
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