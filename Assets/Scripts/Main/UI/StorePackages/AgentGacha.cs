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
        private GachaInfo _gachaInfo;

        [SerializeField]
        private GameObject skipButton; 
        [SerializeField]
        private GameObject nextBtn;

        private int _count; // 최대 아이템 뜰 개수

        [SerializeField]
        private Sprite _backBadgeImage; // 뱃지 뒷면 

        public List<GachaCard> gachaCards = new List<GachaCard>(); // 총 아이템개수 
        public List<GachaCard> curGachaCards = new List<GachaCard>(); // 현재 뽑을 아이템 개수 
         
        private int currentNum; // 현재 몇번째 아이템 강조중 
        private int currentAmount; // 현재 총 뽑은 아이템 수 
        private bool isCost = true; // 돈이 충분한상태인가 

   

        void Start()
        {
            ListenEvent();
            _gachaInfo.allItemInfos.SetInfosGrade();
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

            EventManager.Instance.StartListening(EventsType.SkipAnimation, SkipAnimation);
            EventManager.Instance.StartListening(EventsType.ActiveNextBtn, ActiveNextBtn);
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
            SummonItems();
        }

        // 일반 80% 희귀 15% 영웅 5%
        [ContextMenu("테스트")]
        private void SummonItems()
        {
            curGachaCards.Clear(); 
       //     gachaCanvas.gameObject.SetActive(true);
            
            AllItemInfos allBadgeInfos = _gachaInfo.allItemInfos;
            float epicPercent = _gachaInfo.gachaSO.epicPercent;
            float rarePercent = _gachaInfo.gachaSO.rarePercent;

            int randomIndex;
            DailyItemInfo getItemInfo;
            for (int i = 0; i < currentAmount; i++)
            {
                int Percent = Random.Range(0, 100 + 1);
                Debug.Log("Percent : " + Percent);
                if (epicPercent >= Percent)
                {
                    //에픽 스티커 소환
                    randomIndex = Random.Range(0, allBadgeInfos.epicItemInfos.Count);
                    getItemInfo = allBadgeInfos.epicItemInfos[randomIndex];
                    Debug.Log($"\"영웅\"등급 {allBadgeInfos.epicItemInfos[randomIndex]} 뱃지가 나왔습니다.");
                }
                else if (rarePercent + epicPercent >= Percent)
                {
                    //레어 스티커 소환
                    randomIndex = Random.Range(0, allBadgeInfos.rareItemInfos.Count);
                    getItemInfo = allBadgeInfos.rareItemInfos[randomIndex];
                    Debug.Log($"\"레어\"등급 {allBadgeInfos.rareItemInfos[randomIndex]} 뱃지가 나왔습니다.");
                }
                else
                {
                    //일반 스티커 소환
                    randomIndex = Random.Range(0, allBadgeInfos.commonItemInfos.Count);
                    getItemInfo = allBadgeInfos.commonItemInfos[randomIndex];
                    Debug.Log($"\"일반\"등급 {allBadgeInfos.commonItemInfos[randomIndex]} 뱃지가 나왔습니다.");
                }
                gachaCards[i].ActiveAndAnimate();
                gachaCards[i].SetSprite(getItemInfo._itemSprite, _backBadgeImage,true);
                curGachaCards.Add(gachaCards[i]);
            }

        }

        /// <summary>
        /// 카드 한장씩 볼 수 있도록 
        /// </summary>
        private void CheckItem()
        {
          //  gachaCards[currentNum].StopCoroutine();

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
        /// 뽑기 초기화
        /// </summary>
        private void InitGacha(int amount)
        {
            currentAmount = amount;
            currentNum = 0;
            skipButton.SetActive(true);
        }

        public void SkipAnimation()
        {
            for(int i =0; i< curGachaCards.Count; i++)
            {
                curGachaCards[i].StopCoroutine();
            }
            ActiveNextBtn();
            skipButton.SetActive(false); 
        }
        /// <summary>
        ///  뽑기 닫기 
        /// </summary>
        private void Close()
        {

            for (int i = 0; i < _count; i++)
            {
                gachaCards[i].gameObject.SetActive(false);
                gachaCards[i].StopCoroutine(); 
            }
            // 검은 이미지 닫기 
           // gachaCanvas.gameObject.SetActive(false);
        }
        /// <summary>
        /// 최대로 뜰 아이템 개수만큼 미리 생성해두기 
        /// </summary>
        private void InstantiateItem()
        {
            _count = _gachaInfo.gachaSO.maxAmount;
            GachaCard itemPrefab = _gachaInfo.itemPrefab;
            GameObject itemParent = _gachaInfo.itemsParent;
            for (int i = 0; i < _count; ++i)
            {
                GachaCard gachaCard = Instantiate(itemPrefab, itemParent.transform);
                gachaCard.gameObject.SetActive(false);
                gachaCards.Add(gachaCard);
            }
        }

        /// <summary>
        /// 다음 아이템 확인하는 버튼 활성화
        /// </summary>
        private void ActiveNextBtn()
        {
            if (currentAmount > 1)
            {
                nextBtn.SetActive(true);
            }
        }

    }

}
