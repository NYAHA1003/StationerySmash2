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
        //변수
        public CardObj SelectedCard => _selectedCard; //선택한 카드

        //변수
        public bool IsSelectCard { get; private set; } = false; //카드를 클릭한 상태인지
        
        //참조 변수
        private CardObj _selectedCard = null;
        private CardComponent _cardComponent;
        private UnitComponent _unitComponent = null;
        private CostComponent _costComponent = null;

        /// <summary>
        /// 초기화
        /// </summary>
        public void SetInitialization(CardComponent cardComponent, UnitComponent unitComponent, CostComponent costComponent)
        {
            this._cardComponent = cardComponent;
            this._unitComponent = unitComponent;
            this._costComponent = costComponent;
        }

        /// <summary>
        /// 카드를 선택함
        /// </summary>
        /// <param name="card"></param>
        public void SelectCard(CardObj card)
        {
            //소환 범위 그리기
            _cardComponent.SetSummonRangeLine(true);

            //해당 카드를 선택된 카드에 넣음
            _selectedCard = card;
            _selectedCard.SetIsSelected(true);

            //카드 크기를 크게 만들고 각도를 0으로 돌림
            card.transform.DOKill();
            card.SetCardScale(Vector3.one * 1.3f, 0.3f);
            card.SetCardRot(Quaternion.identity, 0.3f);

            //카드 선택 활성화
            IsSelectCard = true;

            //카드 융합 설정
            _cardComponent.SetDelayFusion();
        }

        /// <summary>
        /// 카드 선택을 취소함
        /// </summary>
        /// <param name="card"></param>
        public void SetUnSelectCard(CardObj card)
        {
            //소환 범위 끄기
            _cardComponent.SetSummonRangeLine(false);

            //융합중이라면 카드 선택 취소를 취소한다
            if (card.IsFusion && !card.IsSelected)
            {
                return;
            }

            //카드 크기를 돌려놓음
            card.SetCardScale(Vector3.one * 1, 0.3f);

            //선택한 카드를 Null로 돌려놓고 카드 선택을 False로 처리함s
            _selectedCard?.SetIsSelected(false);
            _selectedCard = null;
            IsSelectCard = false;

            _cardComponent.SetDelayFusion();
        }

        /// <summary>
        /// 카드를 사용한다
        /// </summary>
        /// <param name="card"></param>
        public bool SetUseCard(CardObj card)
        {
            //카드를 사용할 수 있는지 체크함
            if (!CheckPossibleSummon())
            {
                card.RunOriginPRS();
                _selectedCard?.SetIsSelected(false);
                _selectedCard = null;
                IsSelectCard = false;
                return false;
            }

            //선택한 카드를 Null로 돌림
            _selectedCard?.SetIsSelected(false);
            _selectedCard = null;

            _costComponent.SubtractCost(card.CardCost);
            _cardComponent.SubtractCardAt(_cardComponent.CardList.FindIndex(x => x.Id == card.Id));
            IsSelectCard = false;

            //카드 사용
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
        /// 선택한 카드 위치를 업데이트 한다
        /// </summary>
        public void UpdateSelectCardPos()
        {
            if (_selectedCard == null)
            {
                return;
            }
            Vector3 position = Input.mousePosition;

            //소환 범위에 들었을 때
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
        /// 선택된 카드를 사용하지 못 하게 한다
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
        /// 카드를 여러 조건에 따라 사용할 수 있는지 체크
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
            //테스트용 소환 조건 해제
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