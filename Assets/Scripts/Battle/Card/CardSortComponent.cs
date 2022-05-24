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
	public class CardSortComponent : BattleComponent
    {
        private CardComponent _cardComponent = null;
        private CardSelectComponent _cardSelectComponent = null;
        private RectTransform _cardLeftPosition = null;
        private RectTransform _cardRightPosition = null;

        /// <summary>
        /// �ʱ�ȭ
        /// </summary>
        public void SetInitialization(CardComponent cardComponent, CardSelectComponent cardSelectComponent, RectTransform cardLeftRectPos, RectTransform cardRightRectPos)
        {
            this._cardComponent = cardComponent;
            this._cardSelectComponent = cardSelectComponent;
            this._cardLeftPosition = cardLeftRectPos;
            this._cardRightPosition = cardRightRectPos;
        }

        /// <summary>
        /// ī�� ��ġ�� ������
        /// </summary>
        public void SortCard()
        {
            //ī�� ��ġ�� ��ȯ�޴´�
            List<PRS> originCardPRS = new List<PRS>();
            originCardPRS = ReturnRoundPRS(_cardComponent.CardList.Count, 800, 600);

            //ī��鿡�� ��ȯ���� ��ġ�� �ִ´�
            for (int i = 0; i < _cardComponent.CardList.Count; i++)
            {
                CardObj targetCard = _cardComponent.CardList[i];
                targetCard.SetOriginPRS(originCardPRS[i]);
                if (_cardComponent.CardList[i].Equals(_cardSelectComponent.SelectedCard))
                {
                    continue;
                }
                targetCard.SetCardPRS(targetCard.OriginPRS, 0.4f);
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
        
    }
}
