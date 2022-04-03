using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Utill;

namespace Battle
{
    [System.Serializable]
    public class CardCommand : BattleCommand
    {
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

        //�ν����� ���� ����
        [SerializeField]
        private LineRenderer _summonRangeLine = null;
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
        private UnitDataSO _unitDataSO = null;
        [SerializeField]
        private StarategyDataSO _starategyDataSO = null;

        //���� ����
        private StageData _stageData = null;
        private CardMove _selectCard = null;
        private DeckData _deckData = null;

        /// <summary>
        /// �ʱ�ȭ
        /// </summary>
        public void SetInitialization(BattleManager battleManager, DeckData deckData, int maxCard)
        {
            //������ ����
            this._battleManager = battleManager;
            this._deckData = deckData;
            this._stageData = battleManager.CurrentStageData;
            this._summonRange = -_stageData.max_Range + _stageData.max_Range / 4;
            SetMaxCard(maxCard);

            //���� ��ȯ ���� �׸���
            DrawSummonRangeLinePos();

            //���� ī�������� ����
            SetDeckCard();

            //������Ʈ�� �Լ��� ����
            battleManager.AddUpdateAction(UpdateUnitAfterImage);
            battleManager.AddUpdateAction(UpdateSelectCardPos);
            battleManager.AddUpdateAction(UpdateCardDraw);
            battleManager.AddUpdateAction(UpdateSummonRange);
        }

        /// <summary>
        /// ���� ī�� �������� �ִ´�
        /// </summary>
        public void SetDeckCard()
        {
            SetUnitCardToDeck();
            SetStrategyCardToDeck();
        }

        /// <summary>
        /// ���� ī�带 ���� ��´�
        /// </summary>
        public void SetUnitCardToDeck()
        {
            for (int i = 0; i < _unitDataSO.unitDatas.Count; i++)
            {
                _deckData.Add_CardData(_unitDataSO.unitDatas[i]);
            }
        }

        /// <summary>
        /// ���� ī�带 ���� ��´�
        /// </summary>
        public void SetStrategyCardToDeck()
        {
            for (int i = 0; i < _starategyDataSO.starategyDatas.Count; i++)
            {
                _deckData.Add_CardData(_starategyDataSO.starategyDatas[i]);
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
            //ī�� �����͸� �������� ������
            int random = Random.Range(0, _deckData.cardDatas.Count);
            _currentCardCount++;

            //ī�带 Ǯ���ؼ� ������
            CardMove cardmove = PoolCard();
            cardmove.Set_UnitData(_deckData.cardDatas[random], _cardIdCount++);

            //ī�� ����Ʈ�� ī�带 ������
            _battleManager._cardDatasTemp.Add(cardmove);

            //ī�� ����� 2�� �̻��̸� ��� ���� ī�尡 ������ �� �ִ��� ī�� ��
            if (_battleManager._cardDatasTemp.Count > 1)
            {
                CardMove targetCard1 = _battleManager._cardDatasTemp[_battleManager._cardDatasTemp.Count - 1];
                CardMove targetCard2 = _battleManager._cardDatasTemp[_battleManager._cardDatasTemp.Count - 2];
                FusionCheck(targetCard1, targetCard2);
            }

            //ī�带 �����ϰ� ���� ������ ����
            SortCard();
            SetDelayFusion();
        }

        /// <summary>
        /// ī�� ��ġ�� ������
        /// </summary>
        public void SortCard()
        {
            //ī�� ��ġ�� ��ȯ�޴´�
            List<PRS> originCardPRS = new List<PRS>();
            originCardPRS = ReturnRoundPRS(_battleManager._cardDatasTemp.Count, 800, 600);

            //ī��鿡�� ��ȯ���� ��ġ�� �ִ´�
            for (int i = 0; i < _battleManager._cardDatasTemp.Count; i++)
            {
                CardMove targetCard = _battleManager._cardDatasTemp[i];
                targetCard._originPRS = originCardPRS[i];
                if (_battleManager._cardDatasTemp[i].Equals(_selectCard))
                {
                    continue;
                }
                targetCard.SetCardPRS(targetCard._originPRS, 0.4f);
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
            SubtractCardAt(_battleManager._cardDatasTemp.FindIndex(x => x._id == cardMove._id));
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
            _battleManager._cardDatasTemp[index].transform.SetParent(_cardPoolManager);
            _battleManager._cardDatasTemp[index].gameObject.SetActive(false);
            _battleManager._cardDatasTemp.RemoveAt(index);

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
                _battleManager.StopCoroutine(_delayCoroutine);
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
            SetSummonRangeLine(true);
            _summonRangeLine.gameObject.SetActive(true);

            //ī�尡 �������̸� ī�� ������ �����
            if (card._isFusion)
            {
                return;
            }
            //�ش� ī�带 ���õ� ī�忡 ����
            _selectCard = card;

            //ī�� ũ�⸦ ũ�� ����� ������ 0���� ����
            _selectCard.transform.DOKill();
            _selectCard.SetCardScale(Vector3.one * 1.3f, 0.3f);
            _selectCard.SetCardRot(Quaternion.identity, 0.3f);
            
            //ī�� ���� Ȱ��ȭ
            IsSelectCard = true;
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
            if (card._isFusion)
            {
                return;
            }
            
            //ī�� ũ�⸦ ��������
            card.SetCardScale(Vector3.one * 1, 0.3f);

            //������ ī�带 Null�� �������� ī�� ������ False�� ó����
            _selectCard = null;
            IsSelectCard = false;

            //ī�带 ���ս�Ŵ
            FusionCard();
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
            if (!CheckPossibleSummon())
            {
                card.RunOriginPRS();
                _battleManager.CommandCamera.SetCameraIsMove(true);
                return;
            }
            //������ ī�带 Null�� ����
            _selectCard = null;

            _battleManager.CommandCost.SubtractCost(card.CardCost);
            SubtractCardAt(_battleManager._cardDatasTemp.FindIndex(x => x._id == card._id));
            IsSelectCard = false;

            //ī�� ���
            Vector3 mouse_Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (_battleManager.CommandUnit.eTeam == TeamType.MyTeam)
            {
                mouse_Pos.x = Mathf.Clamp(mouse_Pos.x, -_stageData.max_Range, _summonRange);
            }


            switch (card._dataBase.cardType)
            {
                case CardType.SummonUnit:
                    _battleManager.CommandUnit.SummonUnit(card._dataBase, new Vector3(mouse_Pos.x, 0, 0), card._grade);
                    break;
                default:
                case CardType.Execute:
                case CardType.SummonTrap:
                case CardType.Installation:
                    card._dataBase.strategyData.starategy_State.Run_Card(_battleManager, _battleManager.CommandUnit.eTeam, card._grade, card._dataBase.strategyData.starategyablityData);
                    break;
            }

            //�� ������ ��ȯ�ϸ� �α׿� �߰���
            //if (_battleManager.CommandUnit.eTeam == TeamType.EnemyTeam)
            //{
            //    _battleManager._aiLog.Add_Log(card._dataBase);
            //}
            
            //ī�� ����
            FusionCard();
        }

        /// <summary>
        /// ī�� ��ȯ �̸�����
        /// </summary>
        /// <param name="unitData"></param>
        /// <param name="pos"></param>
        /// <param name="isDelete"></param>
        public void UpdateUnitAfterImage()
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (_battleManager.CommandUnit.eTeam == TeamType.MyTeam)
                pos.x = Mathf.Clamp(pos.x, -_stageData.max_Range, _summonRange);
            if (_selectCard == null || _selectCard._dataBase.unitData.unitType == UnitType.None || pos.y < 0)
            {
                _unitAfterImage.SetActive(false);
                return;
            }
            _unitAfterImage.SetActive(true);
            _afterImageSpriteRenderer.color = Color.white;
            if (CheckPossibleSummon())
            {
                _afterImageSpriteRenderer.color = Color.red;
            }
            _unitAfterImage.transform.position = new Vector3(pos.x, 0);
            _afterImageSpriteRenderer.sprite = _selectCard._dataBase.card_Sprite;
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
            if (_battleManager.isAnySummon)
            {
                return true;
            }
            if (_battleManager.CommandUnit.eTeam.Equals(TeamType.EnemyTeam))
            {
                return true;
            }

            switch (_selectCard._dataBase.cardType)
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

            if (_battleManager.CommandCost.CurrentCost < _selectCard.CardCost)
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
            _summonRange = _summonRange + _stageData.max_Range / 4;
            DrawSummonRangeLinePos();
        }

        /// <summary>
        /// ��ȯ ���� �׸��⸦ Ű�ų� ����
        /// </summary>
        /// <param name="isActive"></param>
        public void SetSummonRangeLine(bool isActive)
        {
            _summonRangeLine.gameObject.SetActive(isActive);
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
        /// ���տ� �����̸� ����, �����ϴ� �Լ�
        /// </summary>
        private void SetDelayFusion()
        {
            //ī�� ���� ������ �ʱ�ȭ
            if (_delayCoroutine != null)
            {
                _battleManager.StopCoroutine(_delayCoroutine);
            }
            _delayCoroutine = _battleManager.StartCoroutine(DelayFusion());
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
            cardmove_obj ??= _battleManager.CreateObject(_cardMovePrefeb, _cardSpawnPosition.position, Quaternion.identity).GetComponent<CardMove>();
            cardmove_obj.transform.SetParent(_cardCanvas);
            cardmove_obj._isFusion = false;
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
            //ī�� �ΰ� �� �ϳ��� ���� Ŭ�� ���� ī������ üũ
            if (targetCard1 == _selectCard || targetCard2 == _selectCard)
            {
                return false;
            }
            //ī�� Ÿ���� ������ üũ
            if (targetCard1._dataBase.cardType != targetCard2._dataBase.cardType)
            {
                return false;
            }
            //���� Ÿ���� ������ üũ
            if (targetCard1._dataBase.unitData.unitType != targetCard2._dataBase.unitData.unitType)
            {
                return false;
            }
            //���� Ÿ���� ������ üũ
            if (targetCard1._dataBase.strategyData.starategyType != targetCard2._dataBase.strategyData.starategyType)
            {
                return false;
            }
            //����� ������ üũ
            if (targetCard1._grade != targetCard2._grade)
            {
                return false;
            }
            if (targetCard1._grade == 3 || targetCard2._grade == 3)
            {
                return false;
            }
            //���������� üũ
            if (targetCard1._isFusion != targetCard2._isFusion)
            {
                return false;
            }

            //���� ���ΰɷ� üũ
            targetCard1._isFusion = true;
            targetCard2._isFusion = true;

            return true;
        }

        /// <summary>
        /// ī�带 ������
        /// </summary>
        private void FusionCard()
        {
            CardMove targetCard1 = null;
            CardMove targetCard2 = null;
            for (int i = 0; i < _battleManager._cardDatasTemp.Count - 1; i++)
            {
                targetCard1 = _battleManager._cardDatasTemp[i];
                targetCard2 = _battleManager._cardDatasTemp[i + 1];
                if (targetCard1._grade > 2 || targetCard2._grade > 2)
                    continue;

                if (FusionCheck(targetCard1, targetCard2))
                {
                    if (_isFusion)
                    {
                        return;
                    }
                    _battleManager.StartCoroutine(FusionMove(i));
                    return;
                }
            }
        }

        /// <summary>
        /// ī�� ���� �ִϸ��̼�
        /// </summary>
        /// <param name="index">�� ��° ī�尡 �����ϴ���</param>
        /// <returns></returns>
        private IEnumerator FusionMove(int index)
        {
            _isFusion = true;
            CardMove targetCard1 = _battleManager._cardDatasTemp[index];
            CardMove targetCard2 = _battleManager._cardDatasTemp[index + 1];
            targetCard1._isFusion = true;
            targetCard2._isFusion = true;

            targetCard2.DOKill();
            targetCard2.SetCardPRS(new PRS(targetCard1.transform.localPosition, targetCard1.transform.rotation, Vector3.one * 0.3f), 0.25f);
            targetCard2._isDontMove = true;

            Color color = targetCard1._grade > 1 ? Color.yellow : Color.white;
            targetCard1.FusionFadeInEffect(color);
            targetCard2.FusionFadeInEffect(color);

            yield return new WaitForSeconds(0.23f);
            targetCard2.ShowCard(false);

            targetCard1.FusionFadeOutEffect();
            targetCard1.UpgradeUnitGrade();

            targetCard1._isFusion = false;
            targetCard2._isFusion = false;
            targetCard2._isDontMove = false;

            SubtractCardFind(targetCard2);
            SortCard();
            _isFusion = false;
        }

        /// <summary>
        /// ��ȯ ���� �ӽ� ���� ������
        /// </summary>
        private void DrawSummonRangeLinePos()
        {
            _summonRangeLine.SetPosition(0, new Vector2(-_stageData.max_Range, 0));
            _summonRangeLine.SetPosition(1, new Vector2(_summonRange, 0));
        }
    }

}