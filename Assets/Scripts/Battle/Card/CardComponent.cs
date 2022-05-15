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
        public List<CardMove> CardList => _cardList;

        //속성
        public bool IsSelectCard { get; private set; } = false; //카드를 클릭한 상태인지

        //기본 변수
        private float _summonRange = 0.0f;
        private bool _isFusion = false;
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
        private CardMove _selectCard = null;
        private DeckData _deckData = new DeckData();
        private List<CardMove> _cardList = new List<CardMove>();
        private UnitComponent _commandUnit = null;
        private CostComponent _commandCost = null;
        private WinLoseComponent _commandWinLose = null;
        private CameraComponent _commandCamera = null;
        private MonoBehaviour _managerBase = null;
        private CardDrowComponent _cardDrawComponent = null;
        private CardRangeComponent _cardRangeComponent = null;
        private CardSelectComponent _cardSelectComponent = null;

        /// <summary>
        /// 초기화
        /// </summary>
        public void SetInitialization(MonoBehaviour managerBase, WinLoseComponent commandWinLose, CameraComponent commandCamera, UnitComponent commandUnit, CostComponent commandCost, ref System.Action updateAction, StageData stageData, int maxCard)
        {
            _cardDrawComponent = new CardDrowComponent();
            _cardRangeComponent = new CardRangeComponent();
            _cardSelectComponent = new CardSelectComponent();

            //변수들 설정
            this._managerBase = managerBase;
            this._stageData = stageData;
            this._summonRange = -_stageData.max_Range + _stageData.max_Range / 4;
            this._commandUnit = commandUnit;
            this._commandCost = commandCost;
            this._commandCamera = commandCamera;
            this._commandWinLose = commandWinLose;

            //관찰자 등록한다
            this._commandWinLose.AddObservers(this);

            SetMaxCard(maxCard);

            //덱에 카드정보들 전달
            SetDeckCard();

            _cardDrawComponent.SetInitialization(_deckData, _cardList, _cardMovePrefeb, _cardPoolManager, _cardCanvas, _cardSpawnPosition);
            _cardRangeComponent.SetInitialization(_summonRangeImage, _stageData);
            _cardSelectComponent.SetInitialization();

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

            if (_selectCard != null)
            {
                _selectCard.DontUseCard();
                _selectCard = null;
            }
        }

        public void CheckCard()
        {
            if(_cardDrawComponent.CheckMaxCard())
            {
                BattleTurtorialComponent.tutorialEventQueue.Dequeue().Invoke(); 
            }
        }

        //카드 뽑기 관련
        /// <summary>
        /// 카드 한 장을 뽑는다
        /// </summary>
        public void DrowOneCard()
		{
            //카드 뽑기
            _cardDrawComponent.AddOneCard();

            //카드를 정렬하고 융합 딜레이 설정
            SortCard();
            SetDelayFusion();

            //카드 뽑을 때 연계 효과들 실행
            RunAction(DrowOneCard);
        }

        /// <summary>
        /// 마지막 카드를 지운다
        /// </summary>
        public void SubtractLastCard()
        {
            SubtractCardAt(_cardDrawComponent.ReturnCurrentCard() - 1);
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

        //카드 선택 관련
        /// <summary>
        /// 카드를 선택함
        /// </summary>
        /// <param name="card"></param>
        public void SelectCard(CardMove card)
        {
            //카드를 사용할 수 없다
            if (_isDontUse)
            {
                return;
            }

            _cardRangeComponent.SetSummonRangeLine(true);
            _summonRangeImage.gameObject.SetActive(true);

            //해당 카드를 선택된 카드에 넣음
            _selectCard = card;

            //카드 크기를 크게 만들고 각도를 0으로 돌림
            _selectCard.transform.DOKill();
            _selectCard.SetCardScale(Vector3.one * 1.3f, 0.3f);
            _selectCard.SetCardRot(Quaternion.identity, 0.3f);
            
            //카드 선택 활성화
            IsSelectCard = true;

            //카드를 융합시킴
            SetDelayFusion();
        }

        /// <summary>
        /// 카드 선택을 취소함
        /// </summary>
        /// <param name="card"></param>
        public void SetUnSelectCard(CardMove card)
        {
            _cardRangeComponent.SetSummonRangeLine(false);

            //융합중이라면 카드 선택 취소를 취소한다
            if (card.IsFusion && card != _selectCard)
            {
                return;
            }
            
            //카드 크기를 돌려놓음
            card.SetCardScale(Vector3.one * 1, 0.3f);

            //선택한 카드를 Null로 돌려놓고 카드 선택을 False로 처리함
            _selectCard = null;
            IsSelectCard = false;

            //카드를 융합시킴
            SetDelayFusion();
        }

        /// <summary>
        /// 카드를 사용한다
        /// </summary>
        /// <param name="card"></param>
        public void SetUseCard(CardMove card)
        {
            //소환할 수 있는지 체크 및 소환 범위 그리기 없앰
            _cardRangeComponent.SetSummonRangeLine(false);

            //카드를 사용할 수 있는지 체크함
            if (!CheckPossibleSummon() || _isDontUse)
            {
                card.RunOriginPRS();
                _commandCamera.SetCameraIsMove(true);
                _selectCard = null;
                IsSelectCard = false;
                return;
            }
            //선택한 카드를 Null로 돌림
            _selectCard = null;

            _commandCost.SubtractCost(card.CardCost);
            SubtractCardAt(_cardList.FindIndex(x => x.Id == card.Id));
            IsSelectCard = false;

            //카드 사용
            Vector3 mouse_Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (_commandUnit.eTeam == TeamType.MyTeam)
            {
                mouse_Pos.x = Mathf.Clamp(mouse_Pos.x, -_stageData.max_Range, _summonRange);
            }


            switch (card.CardDataValue.cardType)
            {
                case CardType.SummonUnit:
                    _commandUnit.SummonUnit(card.CardDataValue, new Vector3(mouse_Pos.x, 0, 0), card.Grade);
                    break;
                default:
                case CardType.Execute:
                case CardType.SummonTrap:
                case CardType.Installation:
                    card.CardDataValue.strategyData.starategy_State.Run_Card(_commandUnit.eTeam);
                    break;
            }

            //카드 융합
            SetDelayFusion();
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

        /// <summary>
        /// 융합에 딜레이를 설정, 리셋하는 함수
        /// </summary>
        private void SetDelayFusion()
        {
            //카드 융합 딜레이 초기화
            if (_delayCoroutine != null)
            {
                _managerBase.StopCoroutine(_delayCoroutine);
            }
            _delayCoroutine = _managerBase.StartCoroutine(DelayFusion());
        }

        /// <summary>
        /// 카드 융합 딜레이를 주는 코루틴 함수
        /// </summary>
        /// <returns></returns>
        private IEnumerator DelayFusion()
        {
            yield return new WaitForSeconds(0.4f);
            FusionCard();
        }

        /// <summary>
        /// 카드 위치를 원형으로 반환함
        /// </summary>
        /// <param name="objCount">카드의 갯수</param>
        /// <param name="y_Space">카드별 y 간격</param>
        /// <param name="std_y_Pos">카드들 위치에서 해당 변수만큼 y위치를 뺌</param>
        /// <returns></returns>
        private List<PRS> ReturnRoundPRS(int objCount, float y_Space, float std_y_Pos)
        {
            float[] objLerps = new float[objCount];
            List<PRS> results = new List<PRS>(objCount);

            //카드 갯수에 따른 예외처리
            switch (objCount)
            {
                case 1:
                    objLerps = new float[] { 0.5f };
                    break;
                case 2:
                    objLerps = new float[] { 0.27f, 0.77f };
                    break;
                default:
                    float interbal = 1f / (objCount - 1 > 0 ? objCount - 1 : 1);
                    for (int i = 0; i < objCount; i++)
                    {
                        objLerps[i] = interbal * i;
                    }
                    break;
            }

            //카드 갯수만큼 갯수 계산해서 위치리스트에 저장
            for (int i = 0; i < objCount; i++)
            {
                Vector3 pos = Vector3.Lerp(_cardLeftPosition.anchoredPosition, _cardRightPosition.anchoredPosition, objLerps[i]);

                float curve = Mathf.Sqrt(Mathf.Pow(1, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                pos.y += curve * y_Space - std_y_Pos;
                Quaternion rot = Quaternion.Slerp(_cardLeftPosition.rotation, _cardRightPosition.rotation, objLerps[i]);
                if (objCount <= 2)
                {
                    rot = Quaternion.identity;
                }

                results.Add(new PRS(pos, rot, Vector3.one));
            }

            return results;
        }

        /// <summary>
        /// 카드의 융합이 가능한지 체크함
        /// </summary>
        /// <param name="targetCard1"></param>
        /// <param name="targetCard2"></param>
        private bool FusionCheck(CardMove targetCard1, CardMove targetCard2)
        {
            //카드 타입이 같은지 체크
            if (targetCard1.CardDataValue.cardType != targetCard2.CardDataValue.cardType)
            {
                return false;
            }
            //유닛 타입이 같은지 체크
            if (targetCard1.CardDataValue.unitData.unitType != targetCard2.CardDataValue.unitData.unitType)
            {
                return false;
            }
            //전략 타입이 같은지 체크
            if (targetCard1.CardDataValue.strategyData.starategyType != targetCard2.CardDataValue.strategyData.starategyType)
            {
                return false;
            }
            //등급이 같은지 체크
            if (targetCard1.Grade != targetCard2.Grade)
            {
                return false;
            }
            if (targetCard1.Grade == 3 || targetCard2.Grade == 3)
            {
                return false;
            }
            //융합중인지 체크
            if (targetCard1.IsFusion != targetCard2.IsFusion)
            {
                return false;
            }

            //융합 중인걸로 체크
            targetCard1.SetIsFusion(true);
            targetCard2.SetIsFusion(true);

            return true;
        }

        /// <summary>
        /// 카드를 융합함
        /// </summary>
        private void FusionCard()
        {
            CardMove targetCard1 = null;
            CardMove targetCard2 = null;
            for (int i = 0; i < _cardList.Count - 1; i++)
            {
                targetCard1 = _cardList[i];
                targetCard2 = _cardList[i + 1];

                if (FusionCheck(targetCard1, targetCard2))
                {
                    if (_isFusion)
                    {
                        return;
                    }
                    _managerBase.StartCoroutine(FusionMove(targetCard1, targetCard2));
                    return;
                }
            }
        }

        /// <summary>
        /// 카드 융합 애니메이션
        /// </summary>
        /// <param name="index">몇 번째 카드가 융합하는지</param>
        /// <returns></returns>
        private IEnumerator FusionMove(CardMove targetCard1, CardMove targetCard2)
        {
            _isFusion = true;
            targetCard1.SetIsFusion(true);
            targetCard2.SetIsFusion(true);

            CardMove toCombineCard = targetCard1;
            CardMove fromCombineCard = targetCard2;

            //두 번째 카드를 선택중일 때
            if (targetCard2 == _selectCard)
            {
                toCombineCard = targetCard2;
                fromCombineCard = targetCard1;
            }
            fromCombineCard.SetIsFusionFrom(true);

            fromCombineCard.DOKill();
            fromCombineCard.SetCardPRS(new PRS(toCombineCard.transform.localPosition, toCombineCard.transform.rotation, Vector3.one * 0.3f), 0.25f);

            Color color = targetCard1.Grade > 1 ? Color.yellow : Color.white;
            toCombineCard.FusionFadeInEffect(color);
            fromCombineCard.FusionFadeInEffect(color);

            yield return new WaitForSeconds(0.23f);
            fromCombineCard.ShowCard(false);

            toCombineCard.FusionFadeOutEffect();
            toCombineCard.UpgradeUnitGrade();

            toCombineCard.SetIsFusion(false);
            fromCombineCard.SetIsFusion(false);
            fromCombineCard.SetIsFusionFrom(false);

            SubtractCardFind(fromCombineCard);
            SortCard();
            _isFusion = false;
        }

        /// <summary>
        /// 카드 위치를 정렬함
        /// </summary>
        private void SortCard()
        {
            //카드 위치를 반환받는다
            List<PRS> originCardPRS = new List<PRS>();
            originCardPRS = ReturnRoundPRS(_cardList.Count, 800, 600);

            //카드들에게 반환받은 위치를 넣는다
            for (int i = 0; i < _cardList.Count; i++)
            {
                CardMove targetCard = _cardList[i];
                targetCard.SetOriginPRS(originCardPRS[i]);
                if (_cardList[i].Equals(_selectCard))
                {
                    continue;
                }
                targetCard.SetCardPRS(targetCard.OriginPRS, 0.4f);
            }
        }

        /// <summary>
        /// 자동 카드 드로우 업데이트
        /// </summary>
        private void UpdateCardDraw()
        {
            if(_cardDrawComponent.CheckCardDraw())
            {
                DrowOneCard();
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
        /// 카드를 찾아서 리스트에서 제거한다
        /// </summary>
        /// <param name="cardMove"></param>
        private void SubtractCardFind(CardMove cardMove)
        {
            SubtractCardAt(_cardList.FindIndex(x => x.Id == cardMove.Id));
        }

        /// <summary>
        /// 지정한 인덱스의 카드를 지운다
        /// </summary>
        private void SubtractCardAt(int index)
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
            SortCard();
        }

        /// <summary>
        /// 선택한 카드 위치를 업데이트 한다
        /// </summary>
        private void UpdateSelectCardPos()
        {
            if (_selectCard == null)
            {
                return;
            }
            _selectCard.transform.position = Input.mousePosition;
        }

        /// <summary>
        /// 카드 소환 미리보기
        /// </summary>
        /// <param name="unitData"></param>
        /// <param name="pos"></param>
        /// <param name="isDelete"></param>
        private void UpdateUnitAfterImage()
        {
            //마우스 위치를 가져온다
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //소환할 유닛이 자신의 유닛인지 체크해서 범위 제한
            if (_commandUnit.eTeam == TeamType.MyTeam)
            {
                pos.x = Mathf.Clamp(pos.x, -_stageData.max_Range, _summonRange);
            }

            //소환 미리보기가 될 수 있는지 체크
            if (_selectCard == null || _selectCard.CardDataValue.unitData.unitType == UnitType.None || pos.y < 0)
            {
                SetSummonArrowImage(false, pos);
                _unitAfterImage.SetActive(false);
                return;
            }

            //소환 미리보기 적용
            _unitAfterImage.SetActive(true);
            _afterImageSpriteRenderer.color = Color.white;

            if (CheckPossibleSummon())
            {
                _afterImageSpriteRenderer.color = Color.red;
            }

            _unitAfterImage.transform.position = new Vector3(pos.x, 0);
            _afterImageSpriteRenderer.sprite = SkinData.GetSkin(_selectCard.CardDataValue._skinData._skinType);

            //소환 화살표 적용
            SetSummonArrowImage(true, pos);
            return;
        }

        /// <summary>
        /// 소환 범위 업데이트 및 증가
        /// </summary>
        private void UpdateSummonRange()
		{
            _cardRangeComponent.UpdateSummonRange();
		}

        /// <summary>
        /// 소환 화살표 설정
        /// </summary>
        private void SetSummonArrowImage(bool isActive, Vector2 pos)
        {
            //소환 화살표 적용
            _summonArrow.gameObject.SetActive(isActive);
            _summonArrow.transform.position = pos;
            _summonArrow.anchoredPosition = new Vector2(_summonArrow.anchoredPosition.x, Mathf.Clamp(_summonArrow.anchoredPosition.y, 520, 1000));
            _summonArrow.sizeDelta = new Vector2(_summonArrow.sizeDelta.x, _summonArrow.anchoredPosition.y);

            return;
        }

        /// <summary>
        /// 카드를 여러 조건에 따라 사용할 수 있는지 체크
        /// </summary>
        private bool CheckPossibleSummon()
        {
            if (_selectCard == null)
            {
                return false;
            }
            //테스트용 소환 조건 해제
            if (_isAlwaysSpawn)
            {
                return true;
            }
            if (_commandUnit.eTeam.Equals(TeamType.EnemyTeam))
            {
                return true;
            }

            switch (_selectCard.CardDataValue.cardType)
            {
                case CardType.Execute:
                    break;
                case CardType.SummonUnit:
                case CardType.SummonTrap:
                    Vector3 mouse_Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mouse_Pos.x = Mathf.Clamp(mouse_Pos.x, -_stageData.max_Range, _summonRange);
                    if (mouse_Pos.x < -_stageData.max_Range || mouse_Pos.x > _summonRange)
                    {
                        return false;
                    }
                    break;
                case CardType.Installation:
                    break;
            }

            if (_commandCost.CurrentCost < _selectCard.CardCost)
            {
                return false;
            }

            return true;
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