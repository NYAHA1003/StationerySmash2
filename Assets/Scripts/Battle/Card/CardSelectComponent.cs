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
	public class CardSelectComponent : BattleComponent
    {
        //����
        public CardObj SelectedCard => _selectedCard; //������ ī��

        //����
        public bool IsSelectCard { get; private set; } = false; //ī�带 Ŭ���� ��������
        
        //���� ����
        private CardObj _selectedCard = null;
        private CardComponent _cardComponent;
        private UnitComponent _unitComponent = null;
        private CostComponent _costComponent = null;

        /// <summary>
        /// �ʱ�ȭ
        /// </summary>
        public void SetInitialization(CardComponent cardComponent, UnitComponent unitComponent, CostComponent costComponent)
        {
            this._cardComponent = cardComponent;
            this._unitComponent = unitComponent;
            this._costComponent = costComponent;
        }

        /// <summary>
        /// ī�带 ������
        /// </summary>
        /// <param name="card"></param>
        public void SelectCard(CardObj card)
        {
            //��ȯ ���� �׸���
            _cardComponent.SetSummonRangeLine(true);

            //�ش� ī�带 ���õ� ī�忡 ����
            _selectedCard = card;
            _selectedCard.SetIsSelected(true);

            //ī�� ũ�⸦ ũ�� ����� ������ 0���� ����
            card.transform.DOKill();
            card.SetCardScale(Vector3.one * 1.3f, 0.3f);
            card.SetCardRot(Quaternion.identity, 0.3f);

            //ī�� ���� Ȱ��ȭ
            IsSelectCard = true;

            //ī�� ���� ����
            _cardComponent.SetDelayFusion();
        }

        /// <summary>
        /// ī�� ������ �����
        /// </summary>
        /// <param name="card"></param>
        public void SetUnSelectCard(CardObj card)
        {
            //��ȯ ���� ����
            _cardComponent.SetSummonRangeLine(false);

            //�������̶�� ī�� ���� ��Ҹ� ����Ѵ�
            if (card.IsFusion && !card.IsSelected)
            {
                return;
            }

            //ī�� ũ�⸦ ��������
            card.SetCardScale(Vector3.one * 1, 0.3f);

            //������ ī�带 Null�� �������� ī�� ������ False�� ó����s
            _selectedCard?.SetIsSelected(false);
            _selectedCard = null;
            IsSelectCard = false;

            _cardComponent.SetDelayFusion();
        }

        /// <summary>
        /// ī�带 ����Ѵ�
        /// </summary>
        /// <param name="card"></param>
        public bool SetUseCard(CardObj card)
        {
            //ī�带 ����� �� �ִ��� üũ��
            if (!CheckPossibleSummon())
            {
                card.RunOriginPRS();
                _selectedCard?.SetIsSelected(false);
                _selectedCard = null;
                IsSelectCard = false;
                return false;
            }

            //������ ī�带 Null�� ����
            _selectedCard?.SetIsSelected(false);
            _selectedCard = null;

            _costComponent.SubtractCost(card.CardCost);
            _cardComponent.SubtractCardAt(_cardComponent.CardList.FindIndex(x => x.Id == card.Id));
            IsSelectCard = false;

            //ī�� ���
            switch (card.CardDataValue._cardType)
            {
                case CardType.SummonUnit:
                    _unitComponent.SummonUnit(card.CardDataValue, new Vector3(_cardComponent.ReturnMinRange(), 0, 0), card.Grade, _unitComponent.eTeam);
                    return true;
                default:
                case CardType.Execute:
                case CardType.SummonTrap:
                case CardType.Installation:
                    StrategyData strategyData = StrategyDataManagerSO.FindStrategyData(card.CardDataValue._starategyType);
                    strategyData.ReturnState().Run_Card(TeamType.MyTeam);
                    return true;
            }
        }

        /// <summary>
        /// ������ ī�� ��ġ�� ������Ʈ �Ѵ�
        /// </summary>
        public void UpdateSelectCardPos()
        {
            if (_selectedCard == null)
            {
                return;
            }
            Vector3 position = Input.mousePosition;

            //��ȯ ������ ����� ��
            switch (_selectedCard.CardDataValue._cardType)
            {
                case CardType.Execute:
                    break;
                case CardType.SummonUnit:
                case CardType.SummonTrap:
                    break;
                case CardType.Installation:
                    break;
            }

            _selectedCard.transform.position = position;
        }

        /// <summary>
        /// ���õ� ī�带 ������� �� �ϰ� �Ѵ�
        /// </summary>
        public void DontUseSelectedCard()
        {
            if (_selectedCard != null)
            {
                _selectedCard.DontUseCard();
                _selectedCard = null;
            }
        }

        /// <summary>
        /// ī�带 ���� ���ǿ� ���� ����� �� �ִ��� üũ
        /// </summary>
        private bool CheckPossibleSummon()
        {
            if (_cardComponent.IsDontUse)
            {
                return false;
            }

            if (_selectedCard == null)
            {
                return false;
            }
            //�׽�Ʈ�� ��ȯ ���� ����
            if (_cardComponent.IsAlwaysSpawn)
            {
                return true;
            }
            if (_unitComponent.eTeam.Equals(TeamType.EnemyTeam))
            {
                return true;
            }

            switch (_selectedCard.CardDataValue._cardType)
            {
                case CardType.Execute:
                    break;
                case CardType.SummonUnit:
                case CardType.SummonTrap:
                    break;
                case CardType.Installation:
                    break;
            }

            if (_costComponent.CurrentCost < _selectedCard.CardCost)
            {
                return false;
            }

            return true;
        }
    }

}