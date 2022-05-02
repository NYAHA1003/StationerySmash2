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
        private int _maxCardCount = 3;
        private int _currentCardCount = 0;
        private float _cardDelay = 0.0f;
        private float _summonRange = 0.0f;
        private float _summonRangeDelay = 30f;
        private bool _isFusion = false;
        private Coroutine _delayCoroutine = null;
        private int _cardIdCount = 0;
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

        /// <summary>
        /// 초기화
        /// </summary>
        public void SetInitialization(MonoBehaviour managerBase, WinLoseComponent commandWinLose, CameraComponent commandCamera, UnitComponent commandUnit, CostComponent commandCost, ref System.Action updateAction, StageData stageData, int maxCard)
        {
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

            //유닛 소환 범위 그리기
            DrawSummonRange();

            //덱에 카드정보들 전달
            SetDeckCard();

            //업데이트할 함수들 전달
            updateAction += UpdateUnitAfterImage;
            updateAction += UpdateSelectCardPos;
            updateAction += UpdateCardDraw;
            updateAction += UpdateSummonRange;
        }

        /// <summary>
        /// 덱에 카드 정보들을 넣는다
        /// </summary>
        public void SetDeckCard()
        {
            int count = _cardDeckSO.cardDatas.Count;
            for(int i = 0; i < count; i++)
            {
                CardData cardData = _cardDeckSO.cardDatas[i];
                _deckData.Add_CardData(cardData);
            }
        }

        /// <summary>
        /// 최대 장수까지 카드를 뽑는다
        /// </summary>
        public void AddAllCard()
        {
            for (; _currentCardCount < _maxCardCount;)
            {
                AddOneCard();
            }
        }

        /// <summary>
        /// 카드 한장을 뽑는다
        /// </summary>
        public void AddOneCard()
        {
            //카드를 사용할 수 없다
            if(_isDontUse)
			{
                return;
			}

            //카드가 없으면 뽑지 않는다
            if(_deckData.cardDatas.Count == 0)
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


            //카드를 정렬하고 융합 딜레이 설정
            SortCard();
            SetDelayFusion();

            RunAction(AddOneCard);
        }

        /// <summary>
        /// 카드 위치를 정렬함
        /// </summary>
        public void SortCard()
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
        /// 마지막 카드를 지운다
        /// </summary>
        public void SubtractLastCard()
        {
            SubtractCardAt(_currentCardCount - 1);
        }
        /// <summary>
        /// 카드를 찾아서 리스트에서 제거한다
        /// </summary>
        /// <param name="cardMove"></param>
        public void SubtractCardFind(CardMove cardMove)
        {
            SubtractCardAt(_cardList.FindIndex(x => x.Id == cardMove.Id));
        }

        /// <summary>
        /// 지정한 인덱스의 카드를 지운다
        /// </summary>
        public void SubtractCardAt(int index)
        {
            if (_currentCardCount == 0)
            {
                return;
            }

            //카드 삭제
            _currentCardCount--;
            _cardList[index].transform.SetParent(_cardPoolManager);
            _cardList[index].gameObject.SetActive(false);
            _cardList.RemoveAt(index);

            //삭제하고 카드를 정렬
            SortCard();
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
            for (; _currentCardCount > 0;)
            {
                SubtractLastCard();
            }
        }

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

            SetSummonRangeLine(true);
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
        /// 선택한 카드 위치를 업데이트 한다
        /// </summary>
        public void UpdateSelectCardPos()
        {
            if (_selectCard == null)
            {
                return;
            }
            _selectCard.transform.position = Input.mousePosition;
        }

        /// <summary>
        /// 카드 선택을 취소함
        /// </summary>
        /// <param name="card"></param>
        public void SetUnSelectCard(CardMove card)
        {
            SetSummonRangeLine(false);

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
            SetSummonRangeLine(false);

            //카드를 사용할 수 있는지 체크함
            if (!CheckPossibleSummon() || _isDontUse)
            {
                card.RunOriginPRS();
                _commandCamera.SetCameraIsMove(true);
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


            switch (card.DataBase.cardType)
            {
                case CardType.SummonUnit:
                    _commandUnit.SummonUnit(card.DataBase, new Vector3(mouse_Pos.x, 0, 0), card.Grade);
                    break;
                default:
                case CardType.Execute:
                case CardType.SummonTrap:
                case CardType.Installation:
                    card.DataBase.strategyData.starategy_State.Run_Card(_commandUnit.eTeam);
                    break;
            }

            //적 유닛을 소환하면 로그에 추가함
            //if (_battleManager.CommandUnit.eTeam == TeamType.EnemyTeam)
            //{
            //    _battleManager._aiLog.Add_Log(card._dataBase);
            //}

            //카드 융합
            SetDelayFusion();
        }

        /// <summary>
        /// 카드 소환 미리보기
        /// </summary>
        /// <param name="unitData"></param>
        /// <param name="pos"></param>
        /// <param name="isDelete"></param>
        public void UpdateUnitAfterImage()
        {
            //마우스 위치를 가져온다
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            //소환할 유닛이 자신의 유닛인지 체크해서 범위 제한
            if (_commandUnit.eTeam == TeamType.MyTeam)
            {
                pos.x = Mathf.Clamp(pos.x, -_stageData.max_Range, _summonRange);
            }

            //소환 미리보기가 될 수 있는지 체크
            if (_selectCard == null || _selectCard.DataBase.unitData.unitType == UnitType.None || pos.y < 0)
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
            _afterImageSpriteRenderer.sprite = SkinData.GetSkin(_selectCard.DataBase.skinData._skinType);

            //소환 화살표 적용
            SetSummonArrowImage(true, pos);
            return;
        }

        /// <summary>
        /// 소환 화살표 설정
        /// </summary>
        public void SetSummonArrowImage(bool isActive, Vector2 pos)
        {
            //소환 화살표 적용
            _summonArrow.gameObject.SetActive(isActive);
            _summonArrow.transform.position = Input.mousePosition;
            _summonArrow.sizeDelta = new Vector2(_summonArrow.sizeDelta.x, _summonArrow.anchoredPosition.y);
            //float ySize = Mathf.Clamp(pos.y * 2f, 0.8f, 2f);
            //_summonArrow.size = new Vector2(0.35f, ySize);
            return;
        }

        /// <summary>
        /// 카드를 여러 조건에 따라 사용할 수 있는지 체크
        /// </summary>
        public bool CheckPossibleSummon()
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

            switch (_selectCard.DataBase.cardType)
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
        /// 소환 범위 업데이트 및 증가
        /// </summary>
        public void UpdateSummonRange()
        {
            if (_summonRange >= 0)
            {
                return;
            }

            if (_summonRangeDelay > 0)
            {
                _summonRangeDelay -= Time.deltaTime;
                return;
            }
            Debug.Log("범위 늘어남");
            _summonRangeDelay = 30f;
            _summonRange += _stageData.max_Range / 4;
            DrawSummonRange();
        }

        /// <summary>
        /// 소환 범위 그리기를 키거나 끄기
        /// </summary>
        /// <param name="isActive"></param>
        public void SetSummonRangeLine(bool isActive)
        {
            _summonRangeImage.gameObject.SetActive(isActive);
        }

        /// <summary>
        /// 자동 카드 드로우 업데이트
        /// </summary>
        public void UpdateCardDraw()
        {
            if (_currentCardCount >= _maxCardCount)
            {
                return;
            }
            if (_cardDelay > 0)
            {
                _cardDelay -= Time.deltaTime;
                return;
            }
            _cardDelay = 0.3f;
            AddOneCard();
        }

        /// <summary>
        /// 최대 카드 설정
        /// </summary>
        /// <param name="max">최대 수</param>
        public void SetMaxCard(int max)
        {
            _maxCardCount = max;
        }

        /// <summary>
        /// 최대 카드 추가
        /// </summary>
        public void AddMaxCard(int add)
        {
            _maxCardCount += add;
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
            if (targetCard1.DataBase.cardType != targetCard2.DataBase.cardType)
            {
                return false;
            }
            //유닛 타입이 같은지 체크
            if (targetCard1.DataBase.unitData.unitType != targetCard2.DataBase.unitData.unitType)
            {
                return false;
            }
            //전략 타입이 같은지 체크
            if (targetCard1.DataBase.strategyData.starategyType != targetCard2.DataBase.strategyData.starategyType)
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
        /// 소환 범위 렌더링
        /// </summary>
        private void DrawSummonRange()
        {
            _summonRangeImage.transform.position = new Vector2(-_stageData.max_Range, 0);
            _summonRangeImage.transform.localScale = new Vector2(Mathf.Abs(_stageData.max_Range + _summonRange), 0.5f);
        }

		public void Notify(bool isWin)
		{
            _isDontUse = true;

            if(_selectCard != null)
			{
                _selectCard.DontUseCard();
                _selectCard = null;
			}
        }
	}

}