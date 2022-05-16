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
        //���� ����
        private CardComponent _cardComponent;
        private MonoBehaviour _managerBase;

        //����
        private bool _isFusion;
        private Coroutine _delayCoroutine = null;

        /// <summary>
        /// �ʱ�ȭ
        /// </summary>
        public void SetInitialization(CardComponent cardComponent, MonoBehaviour managerBase)
		{
            _cardComponent = cardComponent;
            _managerBase = managerBase;
		}

        /// <summary>
        /// ���տ� �����̸� ����, �����ϴ� �Լ�
        /// </summary>
        public void SetFusionAndDelay()
        {
            //ī�� ���� ������ �ʱ�ȭ
            if (_delayCoroutine != null)
            {
                _managerBase.StopCoroutine(_delayCoroutine);
            }
            _delayCoroutine = _managerBase.StartCoroutine(DelayFusion());
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
        /// ī�� ���� �����̸� �ִ� �ڷ�ƾ �Լ�
        /// </summary>
        /// <returns></returns>
        private IEnumerator DelayFusion()
        {
            yield return new WaitForSeconds(0.4f);
            FusionCard();
        }
    }

}