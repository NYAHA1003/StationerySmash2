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


        private int _count; // 최대 아이템 뜰 개수

        [SerializeField]
        private Sprite _backBadgeImage; // 뱃지 뒷면 
        public List<GachaCard> gachaCards = new List<GachaCard>();

        private int currentNum; // 현재 몇번째 아이템 뽑는중 
        private int currentAmount; // 현재 총 뽑을 아이템 수 
        private bool isCost = true; // 돈이 충분한상태인가 

        private int totalCount; // 총 뽑기횟수
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
        /// 이벤트매니저에 이벤트 등록 
        /// </summary>
        private void ListenEvent()
        {
            EventManager.Instance.StartListening(EventsType.CloseGacha, Close);
            EventManager.Instance.StartListening(EventsType.CheckItem, CheckItem);
            EventManager.Instance.StartListening(EventsType.CheckCost, (x) => CheckCost((int)x));
            EventManager.Instance.StartListening(EventsType.StartGacha, (x) => Summons((int)x));
        }

        /// <summary>
        /// 현재 가진 돈이 충분한가 
        /// </summary>
        /// <param name="cost"></param>
        private void CheckCost(int cost) // 10, 20 ,40 단위 
        {
            /*
             * if(현재 가진 달고나 < cost) 
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

        // 일반 80% 희귀 15% 영웅 5%
        [ContextMenu("테스트")]
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
        /// 카드 한장씩 볼 수 있도록 
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
        /// 뽑기 캔버스 초기화
        /// </summary>
        private void InitGacha(int amount)
        {
            currentAmount = amount;
            currentNum = 0;
        }
        /// <summary>
        ///  뽑기 닫기 
        /// </summary>
        private void Close()
        {

            for (int i = 0; i < _count; i++)
            {
                gachaCards[i].gameObject.SetActive(false);
            }
            // 검은 이미지 닫기 
            blackBackImage.gameObject.SetActive(false);
        }
        /// <summary>
        /// 최대로 뜰 아이템 개수만큼 미리 생성해두기 
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
