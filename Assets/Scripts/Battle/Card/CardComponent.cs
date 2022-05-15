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
        private float _summonRange = 0.0f;
        private bool _isFusion = false;
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
        /// �ʱ�ȭ
        /// </summary>
        public void SetInitialization(MonoBehaviour managerBase, WinLoseComponent commandWinLose, CameraComponent commandCamera, UnitComponent commandUnit, CostComponent commandCost, ref System.Action updateAction, StageData stageData, int maxCard)
        {
            _cardDrawComponent = new CardDrowComponent();
            _cardRangeComponent = new CardRangeComponent();
            _cardSelectComponent = new CardSelectComponent();

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

            //���� ī�������� ����
            SetDeckCard();

            _cardDrawComponent.SetInitialization(_deckData, _cardList, _cardMovePrefeb, _cardPoolManager, _cardCanvas, _cardSpawnPosition);
            _cardRangeComponent.SetInitialization(_summonRangeImage, _stageData);
            _cardSelectComponent.SetInitialization();

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

        //ī�� �̱� ����
        /// <summary>
        /// ī�� �� ���� �̴´�
        /// </summary>
        public void DrowOneCard()
		{
            //ī�� �̱�
            _cardDrawComponent.AddOneCard();

            //ī�带 �����ϰ� ���� ������ ����
            SortCard();
            SetDelayFusion();

            //ī�� ���� �� ���� ȿ���� ����
            RunAction(DrowOneCard);
        }

        /// <summary>
        /// ������ ī�带 �����
        /// </summary>
        public void SubtractLastCard()
        {
            SubtractCardAt(_cardDrawComponent.ReturnCurrentCard() - 1);
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

        //ī�� ���� ����
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

            _cardRangeComponent.SetSummonRangeLine(true);
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
        /// ī�� ������ �����
        /// </summary>
        /// <param name="card"></param>
        public void SetUnSelectCard(CardMove card)
        {
            _cardRangeComponent.SetSummonRangeLine(false);

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
            _cardRangeComponent.SetSummonRangeLine(false);

            //ī�带 ����� �� �ִ��� üũ��
            if (!CheckPossibleSummon() || _isDontUse)
            {
                card.RunOriginPRS();
                _commandCamera.SetCameraIsMove(true);
                _selectCard = null;
                IsSelectCard = false;
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

            //ī�� ����
            SetDelayFusion();
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
            if (targetCard1.CardDataValue.cardType != targetCard2.CardDataValue.cardType)
            {
                return false;
            }
            //���� Ÿ���� ������ üũ
            if (targetCard1.CardDataValue.unitData.unitType != targetCard2.CardDataValue.unitData.unitType)
            {
                return false;
            }
            //���� Ÿ���� ������ üũ
            if (targetCard1.CardDataValue.strategyData.starategyType != targetCard2.CardDataValue.strategyData.starategyType)
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
        /// ī�� ��ġ�� ������
        /// </summary>
        private void SortCard()
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
        /// �ڵ� ī�� ��ο� ������Ʈ
        /// </summary>
        private void UpdateCardDraw()
        {
            if(_cardDrawComponent.CheckCardDraw())
            {
                DrowOneCard();
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
        /// ī�带 ã�Ƽ� ����Ʈ���� �����Ѵ�
        /// </summary>
        /// <param name="cardMove"></param>
        private void SubtractCardFind(CardMove cardMove)
        {
            SubtractCardAt(_cardList.FindIndex(x => x.Id == cardMove.Id));
        }

        /// <summary>
        /// ������ �ε����� ī�带 �����
        /// </summary>
        private void SubtractCardAt(int index)
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
            SortCard();
        }

        /// <summary>
        /// ������ ī�� ��ġ�� ������Ʈ �Ѵ�
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
        /// ī�� ��ȯ �̸�����
        /// </summary>
        /// <param name="unitData"></param>
        /// <param name="pos"></param>
        /// <param name="isDelete"></param>
        private void UpdateUnitAfterImage()
        {
            //���콺 ��ġ�� �����´�
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //��ȯ�� ������ �ڽ��� �������� üũ�ؼ� ���� ����
            if (_commandUnit.eTeam == TeamType.MyTeam)
            {
                pos.x = Mathf.Clamp(pos.x, -_stageData.max_Range, _summonRange);
            }

            //��ȯ �̸����Ⱑ �� �� �ִ��� üũ
            if (_selectCard == null || _selectCard.CardDataValue.unitData.unitType == UnitType.None || pos.y < 0)
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
            _afterImageSpriteRenderer.sprite = SkinData.GetSkin(_selectCard.CardDataValue._skinData._skinType);

            //��ȯ ȭ��ǥ ����
            SetSummonArrowImage(true, pos);
            return;
        }

        /// <summary>
        /// ��ȯ ���� ������Ʈ �� ����
        /// </summary>
        private void UpdateSummonRange()
		{
            _cardRangeComponent.UpdateSummonRange();
		}

        /// <summary>
        /// ��ȯ ȭ��ǥ ����
        /// </summary>
        private void SetSummonArrowImage(bool isActive, Vector2 pos)
        {
            //��ȯ ȭ��ǥ ����
            _summonArrow.gameObject.SetActive(isActive);
            _summonArrow.transform.position = pos;
            _summonArrow.anchoredPosition = new Vector2(_summonArrow.anchoredPosition.x, Mathf.Clamp(_summonArrow.anchoredPosition.y, 520, 1000));
            _summonArrow.sizeDelta = new Vector2(_summonArrow.sizeDelta.x, _summonArrow.anchoredPosition.y);

            return;
        }

        /// <summary>
        /// ī�带 ���� ���ǿ� ���� ����� �� �ִ��� üũ
        /// </summary>
        private bool CheckPossibleSummon()
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