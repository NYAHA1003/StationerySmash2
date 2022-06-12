using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
using TMPro;
using Utill.Data;
using Main.Event;
using DG.Tweening;


namespace Main.Store
{
    public class StickerGacha1 : MonoBehaviour
    {
        [SerializeField]
        private GachaInfo gachaInfo;
        [SerializeField]
        private Canvas gachaCanvas;
        [SerializeField]
        private Image blackBackImage;


        private int _count; // �ִ� ������ �� ����

        [SerializeField]
        private Sprite _backBadgeImage; // ���� �޸� 
        public List<GachaCard> gachaCards = new List<GachaCard>();

        private int currentNum; // ���� ���° ������ �̴��� 
        private int currentAmount; // ���� �� ���� ������ �� 
        private bool isCost = true; // ���� ����ѻ����ΰ� 

        private int totalCount; // �� �̱�Ƚ��
        private int commonCount;
        private int rareCount;
        private int epicCount;

        private int RandomNum;

        void Start()
        {
            ListenEvent();
            gachaInfo.allItemInfos.SetInfosGrade();
            InstantiateItem();
        }

        /// <summary>
        /// �̺�Ʈ�Ŵ����� �̺�Ʈ ��� 
        /// </summary>
        private void ListenEvent()
        {
            EventManager.Instance.StartListening(EventsType.CloseGacha, Close);
            EventManager.Instance.StartListening(EventsType.CheckItem, CheckItem);
            EventManager.Instance.StartListening(EventsType.CheckCost, (x) => CheckCost((int)x));
            EventManager.Instance.StartListening(EventsType.StartGacha, (x) => Summons((int)x));
        }

        /// <summary>
        /// ���� ���� ���� ����Ѱ� 
        /// </summary>
        /// <param name="cost"></param>
        private void CheckCost(int cost) // 10, 20 ,40 ���� 
        {
            /*
             * if(���� ���� �ް� < cost) 
             * {
             *      isCost = false; 
             *      return; 
             * }
             * isCost = true; 
             */
        }

        private void Summons(int amount)
        {
            if (isCost == false)
            {
                return;
            }
            InitGacha(amount);
            ItemSummons(amount);
        }

        // �Ϲ� 80% ��� 15% ���� 5%
        [ContextMenu("�׽�Ʈ")]
        private void ItemSummons(int amount)
        {
            int epicPercent;

            switch (amount)
            {
                case 5:
                    commonCount = 4;
                    rareCount = 1;
                    totalCount = commonCount + rareCount; 

                    epicPercent = Random.Range(1, 5); 
                    if(epicPercent == 1)
                    {

                    }

                    break;

                case 10:

                    break;

                case 20:

                    break; 
            }

            AllItemInfos allBadgeInfos = gachaInfo.allItemInfos;

            for (int i = 0; i < currentAmount; i++)
            {
                int Percent = Random.Range(0, 100 + 1);
                Debug.Log("Percent : " + Percent);
             
                DailyItemInfo getItemInfo = allBadgeInfos.commonItemInfos[RandomNum];
                gachaCards[i].ActiveAndAnimate();
                gachaCards[i].SetSprite(getItemInfo._itemSprite, _backBadgeImage, true);
            }
       //     gachaCards.Add(gachaCard);
        }

        /// <summary>
        /// ī�� ���徿 �� �� �ֵ��� 
        /// </summary>
        private void CheckItem()
        {
            if (currentNum == currentAmount - 1 || currentAmount == 1)
            {
                return;
            }
            for (int i = 0; i < currentAmount; i++)
            {
                gachaCards[i].gameObject.SetActive(false);
            }

            gachaCards[++currentNum].StressOneItem();
        }

        /// <summary>
        /// �̱� ĵ���� �ʱ�ȭ
        /// </summary>
        private void InitGacha(int amount)
        {
            currentAmount = amount;
            currentNum = 0;
        }
        /// <summary>
        ///  �̱� �ݱ� 
        /// </summary>
        private void Close()
        {

            for (int i = 0; i < _count; i++)
            {
                gachaCards[i].gameObject.SetActive(false);
            }
            // ���� �̹��� �ݱ� 
            blackBackImage.gameObject.SetActive(false);
        }
        /// <summary>
        /// �ִ�� �� ������ ������ŭ �̸� �����صα� 
        /// </summary>
        private void InstantiateItem()
        {
            _count = gachaInfo.gachaSO.maxAmount;
            GachaCard itemPrefab = gachaInfo.itemPrefab;
            GameObject itemParent = gachaInfo.itemsParent;
            GachaCard gachaCard = Instantiate(itemPrefab, itemParent.transform);
            gachaCard.gameObject.SetActive(false);

        }

    }

}
