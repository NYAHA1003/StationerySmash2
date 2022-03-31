using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Utill;

namespace Battle
{
    public class CardCommand : BattleCommand
    {
        private int _maxCard = 3;
        private int _curCard = 0;
        private float _cardDelay = 0.0f;
        private float _summonRange = 0.0f;
        private float _summonRangeDelay = 30f;
        private LineRenderer _summonRangeLine = null;
        private DeckData _deckData = null;
        private UnitDataSO _unitDataSO = null;
        private StarategyDataSO _starategyDataSO = null;
        private StageData _stageData = null;
        private GameObject _cardMovePrefeb = null;
        private Transform _cardPoolManager = null;
        private Transform _cardCanvas = null;
        private RectTransform _cardLeftPosition = null;
        private RectTransform _cardRightPosition = null;
        private RectTransform _cardSpawnPosition = null;

        private GameObject _unitAfterImage = null;
        private SpriteRenderer _afterImageSpriteRenderer = null;

        private CardMove _selectCard = null;

        public bool IsCardDown { get; private set; } = false; //ī�带 Ŭ���� ��������
        public bool IsPossibleSummon { get; private set; } = false; //�ش� ī�带 ��ȯ�� �� �ִ���
        private bool _isFusion = false;

        Coroutine _delayCoroutine = null;

        private int _cardIdCount = 0;

        public void SetInitialization(BattleManager battleManager, DeckData deckData, UnitDataSO unitDataSO, StarategyDataSO starategyDataSO, GameObject card_Prefeb, Transform card_PoolManager, Transform card_Canvas, RectTransform card_SpawnPosition, RectTransform card_LeftPosition, RectTransform card_RightPosition, GameObject unit_AfterImage, LineRenderer summonRangeLine)
        {
            SetBattleManager(battleManager);
            this._deckData = deckData;
            this._unitDataSO = unitDataSO;
            this._starategyDataSO = starategyDataSO;
            this._cardMovePrefeb = card_Prefeb;
            this._cardPoolManager = card_PoolManager;
            this._cardCanvas = card_Canvas;
            this._cardSpawnPosition = card_SpawnPosition;
            this._cardRightPosition = card_RightPosition;
            this._cardLeftPosition = card_LeftPosition;
            this._stageData = battleManager.CurrentStageData;
            this._summonRange = -_stageData.max_Range + _stageData.max_Range / 4;
            this._summonRangeLine = summonRangeLine;

            SetSummonRangeLinePos();

            this._unitAfterImage = unit_AfterImage;
            _afterImageSpriteRenderer = unit_AfterImage.GetComponent<SpriteRenderer>();

            SetDeckCard();

            battleManager.AddAction(UpdateUnitAfterImage);
            battleManager.AddAction(UpdateSelectCardPos);
            battleManager.AddAction(UpdateCardDrow);
            battleManager.AddAction(CheckPossibleSummon);
            battleManager.AddAction(UpdateSummonRange);
        }

        /// <summary>
        /// ���� ī�� �������� �ִ´�(�ӽ�)
        /// </summary>
        public void SetDeckCard()
        {
            for (int i = 0; i < _unitDataSO.unitDatas.Count; i++)
            {
                _deckData.Add_CardData(_unitDataSO.unitDatas[i]);
            }
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
            for (; _curCard < _maxCard;)
            {
                AddOneCard();
            }
        }

        /// <summary>
        /// ī�� ������ �̴´�
        /// </summary>
        public void AddOneCard()
        {
            int random = Random.Range(0, _deckData.cardDatas.Count);
            _curCard++;

            CardMove cardmove = PoolCard();
            cardmove._isFusion = false;
            cardmove.Set_UnitData(_deckData.cardDatas[random], _cardIdCount++);
            battleManager._cardDatasTemp.Add(cardmove);

            if (battleManager._cardDatasTemp.Count > 1)
            {
                CardMove targetCard1 = battleManager._cardDatasTemp[battleManager._cardDatasTemp.Count - 1];
                CardMove targetCard2 = battleManager._cardDatasTemp[battleManager._cardDatasTemp.Count - 2];

                if (targetCard1._grade < 2 && targetCard2._grade < 2)
                    FusionCheck(targetCard1, targetCard2);
            }

            SortCard();
            if (_delayCoroutine != null)
                battleManager.StopCoroutine(_delayCoroutine);
            _delayCoroutine = battleManager.StartCoroutine(DelayDrow());
        }

        private IEnumerator DelayDrow()
        {
            yield return new WaitForSeconds(0.4f);
            FusionCard();
        }


        /// <summary>
        /// ī�带 Ǯ����
        /// </summary>
        private CardMove PoolCard()
        {
            GameObject cardmove_obj = null;
            if (_cardPoolManager.childCount > 0)
            {
                cardmove_obj = _cardPoolManager.GetChild(0).gameObject;
                cardmove_obj.transform.position = _cardSpawnPosition.position;
                cardmove_obj.SetActive(true);
            }
            cardmove_obj ??= battleManager.CreateObject(_cardMovePrefeb, _cardSpawnPosition.position, Quaternion.identity);
            cardmove_obj.transform.SetParent(_cardCanvas);
            return cardmove_obj.GetComponent<CardMove>();
        }

        /// <summary>
        /// ī�� ��ġ�� ������
        /// </summary>
        public void SortCard()
        {
            List<PRS> originCardPRS = new List<PRS>();
            originCardPRS = ReturnRoundPRS(battleManager._cardDatasTemp.Count, 800, 600);

            for (int i = 0; i < battleManager._cardDatasTemp.Count; i++)
            {
                CardMove targetCard = battleManager._cardDatasTemp[i];
                targetCard._originPRS = originCardPRS[i];
                if (battleManager._cardDatasTemp[i].Equals(_selectCard))
                    continue;
                //if (battleManager.card_DatasTemp[i].isFusion)
                //  continue;
                targetCard.SetCardPRS(targetCard._originPRS, 0.4f);
            }
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
                return false;
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

            if (targetCard1._isFusion != targetCard2._isFusion)
            {
                return false;
            }
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
            for (int i = 0; i < battleManager._cardDatasTemp.Count - 1; i++)
            {
                targetCard1 = battleManager._cardDatasTemp[i];
                targetCard2 = battleManager._cardDatasTemp[i + 1];
                if (targetCard1._grade > 2 || targetCard2._grade > 2)
                    continue;

                if (FusionCheck(targetCard1, targetCard2))
                {
                    if (_isFusion)
                        return;
                    battleManager.StartCoroutine(FusionMove(i));
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
            CardMove targetCard1 = battleManager._cardDatasTemp[index];
            CardMove targetCard2 = battleManager._cardDatasTemp[index + 1];
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
        /// �ֱ� ���� ī�带 �����
        /// </summary>
        public void SubtractCard()
        {
            SubtractCardAt(_curCard - 1);
        }
        public void SubtractCardFind(CardMove cardMove)
        {
            SubtractCardAt(battleManager._cardDatasTemp.FindIndex(x => x._id == cardMove._id));
        }

        /// <summary>
        /// ������ �ε����� ī�带 �����
        /// </summary>
        public void SubtractCardAt(int index)
        {
            if (_curCard.Equals(0))
                return;

            _curCard--;
            battleManager._cardDatasTemp[index].transform.SetParent(_cardPoolManager);
            battleManager._cardDatasTemp[index].gameObject.SetActive(false);
            battleManager._cardDatasTemp.RemoveAt(index);
            SortCard();
        }

        /// <summary>
        /// ��� ī�带 �����
        /// </summary>
        public void ClearCards()
        {
            if (_delayCoroutine != null)
                battleManager.StopCoroutine(_delayCoroutine);
            for (; _curCard > 0;)
            {
                SubtractCard();
            }
        }

        /// <summary>
        /// ī�带 ������
        /// </summary>
        /// <param name="card"></param>
        public void SetSelectCard(CardMove card)
        {
            _summonRangeLine.gameObject.SetActive(true);
            if (card._isFusion) return;
            card.transform.DOKill();
            card.SetCardScale(Vector3.one * 1.3f, 0.3f);
            card.SetCardRot(Quaternion.identity, 0.3f);
            _selectCard = card;
            IsCardDown = true;
        }

        /// <summary>
        /// ������ ī�� ��ġ�� ������Ʈ �Ѵ�
        /// </summary>
        public void UpdateSelectCardPos()
        {
            if (_selectCard == null)
                return;
            _selectCard.transform.position = Input.mousePosition;
        }

        /// <summary>
        /// ī�� ������ �����
        /// </summary>
        /// <param name="card"></param>
        public void SetUnSelectCard(CardMove card)
        {
            _summonRangeLine.gameObject.SetActive(false);
            if (card._isFusion) return;
            card.SetCardScale(Vector3.one * 1, 0.3f);
            _selectCard = null;
            IsCardDown = false;
            FusionCard();
        }

        /// <summary>
        /// ī�带 ����Ѵ�
        /// </summary>
        /// <param name="card"></param>
        public void SetUseCard(CardMove card)
        {
            _selectCard = null;
            _summonRangeLine.gameObject.SetActive(false);

            CheckPossibleSummon();
            if (!IsPossibleSummon)
            {
                card.RunOriginPRS();
                battleManager.CommandCamera.SetCameraIsMove(true);
                return;
            }

            battleManager.CommandCost.SubtractCost(card.CardCost);
            SubtractCardAt(battleManager._cardDatasTemp.FindIndex(x => x._id == card._id));
            IsCardDown = false;

            //ī�� ���
            Vector3 mouse_Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (battleManager.CommandUnit.eTeam == TeamType.MyTeam)
                mouse_Pos.x = Mathf.Clamp(mouse_Pos.x, -_stageData.max_Range, _summonRange);

            switch (card._dataBase.cardType)
            {
                case CardType.SummonUnit:
                    battleManager.CommandUnit.SummonUnit(card._dataBase, new Vector3(mouse_Pos.x, 0, 0), card._grade);
                    break;
                default:
                case CardType.Execute:
                case CardType.SummonTrap:
                case CardType.Installation:
                    card._dataBase.strategyData.starategy_State.Run_Card(battleManager, battleManager.CommandUnit.eTeam, card._grade, card._dataBase.strategyData.starategyablityData);
                    break;
            }
            if (battleManager.CommandUnit.eTeam == TeamType.EnemyTeam)
            {
                battleManager._aiLog.Add_Log(card._dataBase);
            }
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
            if (battleManager.CommandUnit.eTeam == TeamType.MyTeam)
                pos.x = Mathf.Clamp(pos.x, -_stageData.max_Range, _summonRange);
            if (_selectCard == null || _selectCard._dataBase.unitData.unitType == UnitType.None || pos.y < 0)
            {
                _unitAfterImage.SetActive(false);
                return;
            }
            _unitAfterImage.SetActive(true);
            _afterImageSpriteRenderer.color = Color.white;
            if (!IsPossibleSummon)
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
        public void CheckPossibleSummon()
        {
            if (_selectCard == null)
                return;
            //�׽�Ʈ�� ��ȯ ���� ����
            if (battleManager.isAnySummon)
            {
                IsPossibleSummon = true;
                return;
            }
            if (battleManager.CommandUnit.eTeam.Equals(TeamType.EnemyTeam))
            {
                IsPossibleSummon = true;
                return;
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
                        IsPossibleSummon = false;
                        return;
                    }
                    break;
                case CardType.Installation:
                    break;
            }

            if (battleManager.CommandCost.CurCost < _selectCard.CardCost)
            {
                IsPossibleSummon = false;
                return;
            }

            IsPossibleSummon = true;
        }

        /// <summary>
        /// ��ȯ ���� ������Ʈ �� ����
        /// </summary>
        public void UpdateSummonRange()
        {
            if (_summonRange >= 0)
                return;

            if (_summonRangeDelay > 0)
            {
                _summonRangeDelay -= Time.deltaTime;
                return;
            }
            Debug.Log("���� �þ");
            _summonRangeDelay = 30f;
            _summonRange = _summonRange + _stageData.max_Range / 4;
            SetSummonRangeLinePos();
        }

        /// <summary>
        /// ��ȯ ���� �ӽ� ���� ������
        /// </summary>
        private void SetSummonRangeLinePos()
        {
            _summonRangeLine.SetPosition(0, new Vector2(-_stageData.max_Range, 0));
            _summonRangeLine.SetPosition(1, new Vector2(_summonRange, 0));
        }

        /// <summary>
        /// �ڵ� ī�� ��ο� ������Ʈ
        /// </summary>
        public void UpdateCardDrow()
        {
            if (_curCard >= _maxCard)
                return;
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
            _maxCard = max;
        }
    }

}