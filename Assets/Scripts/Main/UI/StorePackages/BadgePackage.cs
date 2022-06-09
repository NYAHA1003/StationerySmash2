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

namespace Main.Store
{
    public class BadgePackage : MonoBehaviour
    {
        [SerializeField]
        private AllBadgeInfos _allBadgeInfos;
        [SerializeField]
        private Canvas gachaCanvas;
        [SerializeField]
        private GameObject itemsParent;
        [SerializeField]
        private GachaCard badgePrefab;
        [SerializeField]
        private Image blackBackImage;
        [SerializeField]
        private GameObject nextBtn; 

        private int _count = 11; // 최대 아이템 뜰 개수

        [SerializeField]
        Button _BadgeButton; // 1개
        [SerializeField]
        Button _BadgeSackButton; // 5개
        [SerializeField]
        Button _BadgeBoxButton; // 11개 

        [SerializeField]
        private Sprite _backBadgeImage; // 뱃지 뒷면 

        private List<GachaCard> gachaCards = new List<GachaCard>();
        // 
        [SerializeField]
        private int _RarePercent = 15;
        [SerializeField]
        private int _EpicPercent = 5;

        private int currentNum; // 현재 몇번째 아이템 강조중 
        private int currentAmount; // 현재 총 뽑은 아이템 수 

        private int RandomNum;

        void Start()
        {
            ListenEvent(); 
            ResetFunctionPakage_UI();
            SetFunctionPakage_UI();
            _allBadgeInfos.SetInfosGrade();
            InstantiateItem();
        }

        /// <summary>
        /// 이벤트 초기화
        /// </summary>
        private void ResetFunctionPakage_UI()
        {
            _BadgeButton.onClick.RemoveAllListeners();
            _BadgeSackButton.onClick.RemoveAllListeners();
            _BadgeBoxButton.onClick.RemoveAllListeners();
        }

        /// <summary>
        /// 이벤트매니저에 이벤트 등록 
        /// </summary>
        private void ListenEvent()
        {
            EventManager.Instance.StartListening(EventsType.CloseGacha, Close);
            EventManager.Instance.StartListening(EventsType.CheckItem, CheckItem); 
        }
        /// <summary>
        /// 이벤트 세팅
        /// </summary>
        private void SetFunctionPakage_UI()
        {
            _BadgeButton.onClick.AddListener(() => Summons(1, 10));
            _BadgeSackButton.onClick.AddListener(() => Summons(5, 20));
            _BadgeBoxButton.onClick.AddListener(() => Summons(11, 40));
        }

        private void Summons(int Amount, int cost)
        {
            /*
             * if(현재 가진 달고나 < cost)
             * {
             *      return; 
             * }
             */
            InitGacha(Amount); 
            BadgeSummons();
        }

        // 일반 80% 희귀 15% 영웅 5%
        [ContextMenu("테스트")]
        private void BadgeSummons()
        {
            blackBackImage.gameObject.SetActive(true);
            for (int i = 0; i< currentAmount; i++)
            {
                int Percent = Random.Range(0, 100 + 1);
                Debug.Log("Percent : " + Percent);
                if (_EpicPercent >= Percent)
                {
                    //에픽 스티커 소환
                    RandomNum = Random.Range(0, _allBadgeInfos.epicItemInfos.Count);
                    Debug.Log($"\"영웅\"등급 {_allBadgeInfos.epicItemInfos[RandomNum]} 뱃지가 나왔습니다.");
                }
                else if (_RarePercent + _EpicPercent >= Percent)
                {
                    //레어 스티커 소환
                    RandomNum = Random.Range(0, _allBadgeInfos.rareItemInfos.Count);
                    Debug.Log($"\"레어\"등급 {_allBadgeInfos.rareItemInfos[RandomNum]} 뱃지가 나왔습니다.");
                }
                else
                {
                    //일반 스티커 소환
                    RandomNum = Random.Range(0, _allBadgeInfos.commonItemInfos.Count);
                    Debug.Log($"\"일반\"등급 {_allBadgeInfos.commonItemInfos[RandomNum]} 뱃지가 나왔습니다.");
                }
                DailyItemInfo getItemInfo = _allBadgeInfos.commonItemInfos[RandomNum];
                gachaCards[i].ActiveAndAnimate(); 
                gachaCards[i].SetSprite(getItemInfo._itemSprite, _backBadgeImage,true);
            }

        }

        /// <summary>
        /// 카드 한장씩 볼 수 있도록 
        /// </summary>
        private void CheckItem()
        {
            if(currentNum == currentAmount - 1)
            {
                nextBtn.SetActive(false);
                return; 
            }
            for(int i = 0; i < currentAmount; i++)
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
            if (amount == 1)
            {
                nextBtn.SetActive(false);
                return; 
            }
            nextBtn.SetActive(true);
        }
        /// <summary>
        ///  뽑기 닫기 
        /// </summary>
        private void Close()
        {
            for(int i =0; i < _count; i++)
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
            for (int i = 0; i < _count; ++i)
            {
                GachaCard gachaCard = Instantiate(badgePrefab, itemsParent.transform);
                gachaCard.gameObject.SetActive(false);
                gachaCards.Add(gachaCard);
            }
        }

    }

}
