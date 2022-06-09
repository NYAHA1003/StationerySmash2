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
    //[Serializable]
    //public class AllBadgeInfos
    //{
    //    public DailyItemSO itemSO;

    //    public List<DailyItemInfo> commonItemInfos = new List<DailyItemInfo>();
    //    public List<DailyItemInfo> rareItemInfos = new List<DailyItemInfo>();
    //    public List<DailyItemInfo> epicItemInfos = new List<DailyItemInfo>();

    //    public void SetInfosGrade()
    //    {
    //        int itemCount = itemSO.dailyItemInfos.Count;

    //        for (int i = 0; i < itemCount; i++)
    //        {
    //            DailyItemInfo badgeInfo = itemSO.dailyItemInfos[i];
    //            if (badgeInfo._grade == Grade.Common)
    //            {
    //                commonItemInfos.Add(badgeInfo);
    //            }
    //            else if (badgeInfo._grade == Grade.Rare)
    //            {
    //                rareItemInfos.Add(badgeInfo);
    //            }
    //            else if (badgeInfo._grade == Grade.Epic)
    //            {
    //                epicItemInfos.Add(badgeInfo);
    //            }
    //        }
    //    }
    //}


    //[Serializable]
    //public class GachaInfo
    //{
    //    public GachaSO gachaSO;
    //    public AllBadgeInfos allBadgeInfos;
    //    public GameObject itemsParent;
    //    public GachaCard itemPrefab;
    //}

    public class AgentGacha : MonoBehaviour
    {
        [SerializeField]
        private GachaInfo gachaInfo; 
        [SerializeField]
        private Canvas gachaCanvas;
        [SerializeField]
        private Image blackBackImage;
        [SerializeField]
        private GameObject nextBtn;

        private int _count; // �ִ� ������ �� ����

        [SerializeField]
        private Sprite _backBadgeImage; // ���� �޸� 

        public List<GachaCard> gachaCards = new List<GachaCard>(); // �� �����۰��� 
        private List<GachaCard> curGachaCards = new List<GachaCard>(); // ���� ���� ������ ���� 
         
        private int currentNum; // ���� ���° ������ ������ 
        private int currentAmount; // ���� �� ���� ������ �� 
        private bool isCost = true; // ���� ����ѻ����ΰ� 

        private int RandomNum;

        void Start()
        {
            ListenEvent();
            gachaInfo.allBadgeInfos.SetInfosGrade();
            InstantiateItem();

        }

        /// <summary>
        /// �̺�Ʈ�Ŵ����� �̺�Ʈ ��� 
        /// </summary>
        private void ListenEvent()
        {
            //EventManager.Instance.StopListening(EventsType.CloseGacha, Close);
            //EventManager.Instance.StopListening(EventsType.CheckItem, CheckItem);
            //EventManager.Instance.StopListening(EventsType.CheckCost, (x) => CheckCost((int)x));
            //EventManager.Instance.StopListening(EventsType.StartGacha, (x) => Summons((int)x));

            EventManager.Instance.StartListening(EventsType.CloseGacha, Close);
            EventManager.Instance.StartListening(EventsType.CheckItem, CheckItem);
            EventManager.Instance.StartListening(EventsType.CheckCost, (x) => CheckCost((int)x));
            EventManager.Instance.StartListening(EventsType.StartGacha, (x) => Summons((int)x));

            EventManager.Instance.StartListening(EventsType.SkipAnimation, SkipAnimation); 
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
            ItemSummons();
        }

        // �Ϲ� 80% ��� 15% ���� 5%
        [ContextMenu("�׽�Ʈ")]
        private void ItemSummons()
        {
            curGachaCards.Clear(); 
            blackBackImage.gameObject.SetActive(true);
            
            AllBadgeInfos allBadgeInfos = gachaInfo.allBadgeInfos;
            float epicPercent = gachaInfo.gachaSO.epicPercent;
            float rarePercent = gachaInfo.gachaSO.rarePercent;

            for (int i = 0; i < currentAmount; i++)
            {
                int Percent = Random.Range(0, 100 + 1);
                Debug.Log("Percent : " + Percent);
                if (epicPercent >= Percent)
                {
                    //���� ��ƼĿ ��ȯ
                    RandomNum = Random.Range(0, allBadgeInfos.epicItemInfos.Count);
                    Debug.Log($"\"����\"��� {allBadgeInfos.epicItemInfos[RandomNum]} ������ ���Խ��ϴ�.");
                }
                else if (rarePercent + epicPercent >= Percent)
                {
                    //���� ��ƼĿ ��ȯ
                    RandomNum = Random.Range(0, allBadgeInfos.rareItemInfos.Count);
                    Debug.Log($"\"����\"��� {allBadgeInfos.rareItemInfos[RandomNum]} ������ ���Խ��ϴ�.");
                }
                else
                {
                    //�Ϲ� ��ƼĿ ��ȯ
                    RandomNum = Random.Range(0, allBadgeInfos.commonItemInfos.Count);
                    Debug.Log($"\"�Ϲ�\"��� {allBadgeInfos.commonItemInfos[RandomNum]} ������ ���Խ��ϴ�.");
                }
                DailyItemInfo getItemInfo = allBadgeInfos.commonItemInfos[RandomNum];
                gachaCards[i].ActiveAndAnimate();
                gachaCards[i].SetSprite(getItemInfo._itemSprite, _backBadgeImage,true);
                curGachaCards.Add(gachaCards[i]);
            }

        }

        /// <summary>
        /// ī�� ���徿 �� �� �ֵ��� 
        /// </summary>
        private void CheckItem()
        {
            gachaCards[currentNum].StopCoroutine();

            if (currentNum == currentAmount - 1)
            {
                nextBtn.SetActive(false);
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
            if(amount == 1)
            {
                nextBtn.SetActive(false);
                return; 
            }
            nextBtn.SetActive(true);
        }

        public void SkipAnimation()
        {
            for(int i =0; i< curGachaCards.Count; i++)
            {
                curGachaCards[i].StopCoroutine();
            }
        }
        /// <summary>
        ///  �̱� �ݱ� 
        /// </summary>
        private void Close()
        {

            for (int i = 0; i < _count; i++)
            {
                gachaCards[i].gameObject.SetActive(false);
                gachaCards[i].StopCoroutine(); 
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
            for (int i = 0; i < _count; ++i)
            {
                GachaCard gachaCard = Instantiate(itemPrefab, itemParent.transform);
                gachaCard.gameObject.SetActive(false);
                gachaCards.Add(gachaCard);
            }
        }

    }

}
