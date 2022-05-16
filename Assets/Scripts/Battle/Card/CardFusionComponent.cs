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
	public class CardFusionComponent : BattleComponent
	{
        //참조 변수
        private CardComponent _cardComponent;
        private MonoBehaviour _managerBase;

        //변수
        private bool _isFusion;
        private Coroutine _delayCoroutine = null;

        /// <summary>
        /// 초기화
        /// </summary>
        public void SetInitialization(CardComponent cardComponent, MonoBehaviour managerBase)
		{
            _cardComponent = cardComponent;
            _managerBase = managerBase;
		}

        /// <summary>
        /// 융합에 딜레이를 설정, 리셋하는 함수
        /// </summary>
        public void SetFusionAndDelay()
        {
            //카드 융합 딜레이 초기화
            if (_delayCoroutine != null)
            {
                _managerBase.StopCoroutine(_delayCoroutine);
            }
            _delayCoroutine = _managerBase.StartCoroutine(DelayFusion());
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
            for (int i = 0; i < _cardComponent.CardList.Count - 1; i++)
            {
                try
                {
                    targetCard1 = _cardComponent.CardList[i];
                    targetCard2 = _cardComponent.CardList[i + 1];
                }
                catch
                {
                    targetCard1 = _cardComponent.CardList[i];
                    targetCard2 = _cardComponent.CardList[i + 1];
                }

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
            if (targetCard2.IsSelected)
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

            _cardComponent.SubtractCardFind(fromCombineCard);
            _cardComponent.SortCard();
            _isFusion = false;
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
    }

}