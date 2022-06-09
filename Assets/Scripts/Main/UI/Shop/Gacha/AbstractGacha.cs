using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utill.Data;
using Random = UnityEngine.Random;
using UnityEngine.UI; 

namespace Main.Store
{
    [Serializable]
    public class AllBadgeInfos
    {
        public DailyItemSO itemSO;

        public List<DailyItemInfo> commonItemInfos = new List<DailyItemInfo>();
        public List<DailyItemInfo> rareItemInfos = new List<DailyItemInfo>();
        public List<DailyItemInfo> epicItemInfos = new List<DailyItemInfo>();

        public void SetInfosGrade()
        {
            int itemCount = itemSO.dailyItemInfos.Count;

            for (int i = 0; i < itemCount; i++)
            {
                DailyItemInfo badgeInfo = itemSO.dailyItemInfos[i];
                if (badgeInfo._grade == Grade.Common)
                {
                    commonItemInfos.Add(badgeInfo);
                }
                else if (badgeInfo._grade == Grade.Rare)
                {
                    rareItemInfos.Add(badgeInfo);
                }
                else if (badgeInfo._grade == Grade.Epic)
                {
                    epicItemInfos.Add(badgeInfo);
                }
            }
        }
    }


    [Serializable]
    public class GachaInfo
    {
        public GachaSO gachaSO;
        public AllBadgeInfos allBadgeInfos;
        public GameObject itemsParent;
        public GachaCard itemPrefab;
    }

    public abstract class AbstractGacha : MonoBehaviour
    {
        [SerializeField]
        protected GachaInfo gachaInfo;
        [SerializeField]
        protected Image blackBackImage;

        [SerializeField]
        protected Sprite _backBadgeImage; // 뱃지 뒷면 
        protected List<   GachaCard> gachaCards = new List<GachaCard>();

        protected int currentNum; // 현재 몇번째 아이템 강조중 
        protected int currentAmount; // 현재 총 뽑은 아이템 수 

        protected int RandomNum;

        public abstract void ItemSummons();
        public abstract void CheckItem();
        public abstract void Summon(int amount);
        /// <summary>
        /// 최대로 뜰 아이템 개수만큼 미리 생성해두기 
        /// </summary>
        protected void InstantiateItem()
        {
            GachaCard itemPrefab = gachaInfo.itemPrefab;
            GameObject itemParent = gachaInfo.itemsParent;
            int _count = gachaCards.Count; 
            for (int i = 0; i < _count; ++i)
            {
                GachaCard gachaCard = Instantiate(itemPrefab, itemParent.transform);
                gachaCard.gameObject.SetActive(false);
                gachaCards.Add(gachaCard);
            }
        }

        /// <summary>
        ///  뽑기 닫기 
        /// </summary>
        protected void Close()
        {
            int _count = gachaCards.Count;
            for (int i = 0; i < _count; i++)
            {
                gachaCards[i].gameObject.SetActive(false);
            }
            // 검은 이미지 닫기 
            blackBackImage.gameObject.SetActive(false);
        }

        /// <summary>
        /// 현재 가진 돈이 충분한가 
        /// </summary>
        /// <param name="cost"></param>
        public bool CheckCost(int cost) // 10, 20 ,40 단위 
        {
            /*
             * if(현재 가진 달고나 < cost) 
             * {
             *      return false; 
             * }
             */
            return true;
        }
    }


}