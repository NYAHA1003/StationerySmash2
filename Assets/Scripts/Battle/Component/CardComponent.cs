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
        public List<CardMove> CardList => _cardList;

        //�Ӽ�
        public bool IsSelectCard { get; private set; } = false; //ī�带 Ŭ���� ��������

        //�⺻ ����
        private int _maxCardCount = 3;
        private int _currentCardCount = 0;
        private float _cardDelay = 0.0f;
        private float _summonRange = 0.0f;
        private float _summonRangeDelay = 30f;
        private bool _isFusion = false;
        private Coroutine _delayCoroutine = null;
        private int _cardIdCount = 0;
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
        private CardMove _selectCard = null;
        private DeckData _deckData = new DeckData();
        private List<CardMove> _cardList = new List<CardMove>();
        private UnitComponent _commandUnit = null;
        private CostComponent _commandCost = null;
        private WinLoseComponent _commandWinLose = null;
        private CameraComponent _commandCamera = null;
        private MonoBehaviour _managerBase = null;

        /// <summary>
        /// �ʱ�ȭ
        /// </summary>
        public void SetInitialization(MonoBehaviour managerBase, WinLoseComponent commandWinLose, CameraComponent commandCamera, UnitComponent commandUnit, CostComponent commandCost, ref System.Action updateAction, StageData stageData, int maxCard)
        {
            //������ ����
            this._managerBase = managerBase;
            this._stageData = stageData;
            this._summonRange = -_stageData.max_Range + _stageData.max_Range / 4;
            this._commandUnit = commandUnit;
            this._commandCost = commandCost;
            this._commandCamera = commandCamera;
            this._commandWinLose = commandWinLose;

            //������ ����Ѵ�
            this._commandWinLose.AddObservers(this);

            SetMaxCard(maxCard);

            //���� ��ȯ ���� �׸���
            DrawSummonRange();

            //���� ī�������� ����
            SetDeckCard();

            //������Ʈ�� �Լ��� ����
            updateAction += UpdateUnitAfterImage;
            updateAction += UpdateSelectCardPos;
            updateAction += UpdateCardDraw;
            updateAction += UpdateSummonRange;
        }

        /// <summary>
        /// ���� ī�� �������� �ִ´�
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
        /// �ִ� ������� ī�带 �̴´�
        /// </summary>
        public void AddAllCard()
        {
            for (; _currentCardCount < _maxCardCount;)
            {
                AddOneCard();
            }
        }

        /// <summary>
        /// ī�� ������ �̴´�
        /// </summary>
        public void AddOneCard()
        {
            //ī�带 ����� �� ����
            if(_isDontUse)
			{
                return;
			}

            //ī�尡 ������ ���� �ʴ´�
            if(_deckData.cardDatas.Count == 0)
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


            //ī�带 �����ϰ� ���� ������ ����
            SortCard();
            SetDelayFusion();

            RunAction(AddOneCard);
        }

        /// <summary>
        /// ī�� ��ġ�� ������
        /// </summary>
        public void SortCard()
        {
            //ī�� ��ġ�� ��ȯ�޴´�
            List<PRS> originCardPRS = new List<PRS>();
            originCardPRS = ReturnRoundPRS(_cardList.Count, 800, 600);

            //ī��鿡�� ��ȯ���� ��ġ�� �ִ´�
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
        /// ������ ī�带 �����
        /// </summary>
        public void SubtractLastCard()
        {
            SubtractCardAt(_currentCardCount - 1);
        }
        /// <summary>
        /// ī�带 ã�Ƽ� ����Ʈ���� �����Ѵ�
        /// </summary>
        /// <param name="cardMove"></param>
        public void SubtractCardFind(CardMove cardMove)
        {
            SubtractCardAt(_cardList.FindIndex(x => x.Id == cardMove.Id));
        }

        /// <summary>
        /// ������ �ε����� ī�带 �����
        /// </summary>
        public void SubtractCardAt(int index)
        {
            if (_currentCardCount == 0)
            {
                return;
            }

            //ī�� ����
            _currentCardCount--;
            _cardList[index].transform.SetParent(_cardPoolManager);
            _cardList[index].gameObject.SetActive(false);
            _cardList.RemoveAt(index);

            //�����ϰ� ī�带 ����
            SortCard();
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
            for (; _currentCardCount > 0;)
            {
                SubtractLastCard();
            }
        }

        /// <summary>
        /// ī�带 ������
        /// </summary>
        /// <param name="card"></param>
        public void SelectCard(CardMove card)
        {
            //ī�带 ����� �� ����
            if (_isDontUse)
            {
                return;
            }

            SetSummonRangeLine(true);
            _summonRangeImage.gameObject.SetActive(true);

            //�ش� ī�带 ���õ� ī�忡 ����
            _selectCard = card;

            //ī�� ũ�⸦ ũ�� ����� ������ 0���� ����
            _selectCard.transform.DOKill();
            _selectCard.SetCardScale(Vector3.one * 1.3f, 0.3f);
            _selectCard.SetCardRot(Quaternion.identity, 0.3f);
            
            //ī�� ���� Ȱ��ȭ
            IsSelectCard = true;

            //ī�带 ���ս�Ŵ
            SetDelayFusion();
        }

        /// <summary>
        /// ������ ī�� ��ġ�� ������Ʈ �Ѵ�
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
        /// ī�� ������ �����
        /// </summary>
        /// <param name="card"></param>
        public void SetUnSelectCard(CardMove card)
        {
            SetSummonRangeLine(false);

            //�������̶�� ī�� ���� ��Ҹ� ����Ѵ�
            if (card.IsFusion && card != _selectCard)
            {
                return;
            }
            
            //ī�� ũ�⸦ ��������
            card.SetCardScale(Vector3.one * 1, 0.3f);

            //������ ī�带 Null�� �������� ī�� ������ False�� ó����
            _selectCard = null;
            IsSelectCard = false;

            //ī�带 ���ս�Ŵ
            SetDelayFusion();
        }

        /// <summary>
        /// ī�带 ����Ѵ�
        /// </summary>
        /// <param name="card"></param>
        public void SetUseCard(CardMove card)
        {
            //��ȯ�� �� �ִ��� üũ �� ��ȯ ���� �׸��� ����
            SetSummonRangeLine(false);

            //ī�带 ����� �� �ִ��� üũ��
            if (!CheckPossibleSummon() || _isDontUse)
            {
                card.RunOriginPRS();
                _commandCamera.SetCameraIsMove(true);
                return;
            }
            //������ ī�带 Null�� ����
            _selectCard = null;

            _commandCost.SubtractCost(card.CardCost);
            SubtractCardAt(_cardList.FindIndex(x => x.Id == card.Id));
            IsSelectCard = false;

            //ī�� ���
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

            //�� ������ ��ȯ�ϸ� �α׿� �߰���
            //if (_battleManager.CommandUnit.eTeam == TeamType.EnemyTeam)
            //{
            //    _battleManager._aiLog.Add_Log(card._dataBase);
            //}

            //ī�� ����
            SetDelayFusion();
        }

        /// <summary>
        /// ī�� ��ȯ �̸�����
        /// </summary>
        /// <param name="unitData"></param>
        /// <param name="pos"></param>
        /// <param name="isDelete"></param>
        public void UpdateUnitAfterImage()
        {
            //���콺 ��ġ�� �����´�
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            //��ȯ�� ������ �ڽ��� �������� üũ�ؼ� ���� ����
            if (_commandUnit.eTeam == TeamType.MyTeam)
            {
                pos.x = Mathf.Clamp(pos.x, -_stageData.max_Range, _summonRange);
            }

            //��ȯ �̸����Ⱑ �� �� �ִ��� üũ
            if (_selectCard == null || _selectCard.DataBase.unitData.unitType == UnitType.None || pos.y < 0)
            {
                SetSummonArrowImage(false, pos);
                _unitAfterImage.SetActive(false);
                return;
            }

            //��ȯ �̸����� ����
            _unitAfterImage.SetActive(true);
            _afterImageSpriteRenderer.color = Color.white;

            if (CheckPossibleSummon())
            {
                _afterImageSpriteRenderer.color = Color.red;
            }

            _unitAfterImage.transform.position = new Vector3(pos.x, 0);
            _afterImageSpriteRenderer.sprite = SkinData.GetSkin(_selectCard.DataBase.skinData._skinType);

            //��ȯ ȭ��ǥ ����
            SetSummonArrowImage(true, pos);
            return;
        }

        /// <summary>
        /// ��ȯ ȭ��ǥ ����
        /// </summary>
        public void SetSummonArrowImage(bool isActive, Vector2 pos)
        {
            //��ȯ ȭ��ǥ ����
            _summonArrow.gameObject.SetActive(isActive);
            _summonArrow.transform.position = Input.mousePosition;
            _summonArrow.sizeDelta = new Vector2(_summonArrow.sizeDelta.x, _summonArrow.anchoredPosition.y);
            //float ySize = Mathf.Clamp(pos.y * 2f, 0.8f, 2f);
            //_summonArrow.size = new Vector2(0.35f, ySize);
            return;
        }

        /// <summary>
        /// ī�带 ���� ���ǿ� ���� ����� �� �ִ��� üũ
        /// </summary>
        public bool CheckPossibleSummon()
        {
            if (_selectCard == null)
            {
                return false;
            }
            //�׽�Ʈ�� ��ȯ ���� ����
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
        /// ��ȯ ���� ������Ʈ �� ����
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
            Debug.Log("���� �þ");
            _summonRangeDelay = 30f;
            _summonRange += _stageData.max_Range / 4;
            DrawSummonRange();
        }

        /// <summary>
        /// ��ȯ ���� �׸��⸦ Ű�ų� ����
        /// </summary>
        /// <param name="isActive"></param>
        public void SetSummonRangeLine(bool isActive)
        {
            _summonRangeImage.gameObject.SetActive(isActive);
        }

        /// <summary>
        /// �ڵ� ī�� ��ο� ������Ʈ
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
        /// �ִ� ī�� ����
        /// </summary>
        /// <param name="max">�ִ� ��</param>
        public void SetMaxCard(int max)
        {
            _maxCardCount = max;
        }

        /// <summary>
        /// �ִ� ī�� �߰�
        /// </summary>
        public void AddMaxCard(int add)
        {
            _maxCardCount += add;
        }
        /// <summary>
        /// ���տ� �����̸� ����, �����ϴ� �Լ�
        /// </summary>
        private void SetDelayFusion()
        {
            //ī�� ���� ������ �ʱ�ȭ
            if (_delayCoroutine != null)
            {
                _managerBase.StopCoroutine(_delayCoroutine);
            }
            _delayCoroutine = _managerBase.StartCoroutine(DelayFusion());
        }

        /// <summary>
        /// ī�� ���� �����̸� �ִ� �ڷ�ƾ �Լ�
        /// </summary>
        /// <returns></returns>
        private IEnumerator DelayFusion()
        {
            yield return new WaitForSeconds(0.4f);
            FusionCard();
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
        /// ī�� ��ġ�� �������� ��ȯ��
        /// </summary>
        /// <param name="objCount">ī���� ����</param>
        /// <param name="y_Space">ī�庰 y ����</param>
        /// <param name="std_y_Pos">ī��� ��ġ���� �ش� ������ŭ y��ġ�� ��</param>
        /// <returns></returns>
        private List<PRS> ReturnRoundPRS(int objCount, float y_Space, float std_y_Pos)
        {
            float[] objLerps = new float[objCount];
            List<PRS> results = new List<PRS>(objCount);

            //ī�� ������ ���� ����ó��
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

            //ī�� ������ŭ ���� ����ؼ� ��ġ����Ʈ�� ����
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
        /// ī���� ������ �������� üũ��
        /// </summary>
        /// <param name="targetCard1"></param>
        /// <param name="targetCard2"></param>
        private bool FusionCheck(CardMove targetCard1, CardMove targetCard2)
        {
            //ī�� Ÿ���� ������ üũ
            if (targetCard1.DataBase.cardType != targetCard2.DataBase.cardType)
            {
                return false;
            }
            //���� Ÿ���� ������ üũ
            if (targetCard1.DataBase.unitData.unitType != targetCard2.DataBase.unitData.unitType)
            {
                return false;
            }
            //���� Ÿ���� ������ üũ
            if (targetCard1.DataBase.strategyData.starategyType != targetCard2.DataBase.strategyData.starategyType)
            {
                return false;
            }
            //����� ������ üũ
            if (targetCard1.Grade != targetCard2.Grade)
            {
                return false;
            }
            if (targetCard1.Grade == 3 || targetCard2.Grade == 3)
            {
                return false;
            }
            //���������� üũ
            if (targetCard1.IsFusion != targetCard2.IsFusion)
            {
                return false;
            }

            //���� ���ΰɷ� üũ
            targetCard1.SetIsFusion(true);
            targetCard2.SetIsFusion(true);

            return true;
        }

        /// <summary>
        /// ī�带 ������
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
        /// ī�� ���� �ִϸ��̼�
        /// </summary>
        /// <param name="index">�� ��° ī�尡 �����ϴ���</param>
        /// <returns></returns>
        private IEnumerator FusionMove(CardMove targetCard1, CardMove targetCard2)
        {
            _isFusion = true;
            targetCard1.SetIsFusion(true);
            targetCard2.SetIsFusion(true);

            CardMove toCombineCard = targetCard1;
            CardMove fromCombineCard = targetCard2;

            //�� ��° ī�带 �������� ��
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
        /// ��ȯ ���� ������
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