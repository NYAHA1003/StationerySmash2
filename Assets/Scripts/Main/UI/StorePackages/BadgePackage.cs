using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
using TMPro;
using DG.Tweening; 
using Utill.Data;

public class BadgeInfo
{
    private string badgeName;
    private Sprite badgeSprite;
}
[Serializable]
public class AllBadgeInfos
{
    public DailyItemSO badgeItemSO;

    public List<DailyItemInfo> badgeCommonInfos = new List<DailyItemInfo>();
    public List<DailyItemInfo> badgeRareInfos = new List<DailyItemInfo>();
    public List<DailyItemInfo> badgeEpicInfos = new List<DailyItemInfo>();

    public void SetInfosGrade()
    {
        int itemCount = badgeItemSO.dailyItemInfos.Count;

        for (int i = 0; i < itemCount; i++)
        {
            DailyItemInfo badgeInfo = badgeItemSO.dailyItemInfos[i]; 
            if(badgeInfo._grade == Grade.Common)
            {
                badgeCommonInfos.Add(badgeInfo);
            }
            else if (badgeInfo._grade == Grade.Rare)
            {
                badgeRareInfos.Add(badgeInfo);
            }
            else if (badgeInfo._grade == Grade.Epic)
            {
                badgeEpicInfos.Add(badgeInfo);
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
        private SaveManager SaveManager;
        [SerializeField]
        private Canvas gachaCanvas;
        [SerializeField]
        private GameObject badgePrefab; 

        //private List<string> BadgeCommonList = new List<string>();
        //private List<string> BadgeRareList = new List<string>();
        //private List<string> BadgeEpicList = new List<string>();

        [SerializeField]
        Button _BadgeButton; // 1개
        [SerializeField]
        Button _BadgeSackButton; // 5개
        [SerializeField]
        Button _BadgeBoxButton; // 11개 

        [SerializeField]
        private Sprite _backBadgeImage; // 뱃지 뒷면 

        // 
        [SerializeField]
        private int _RarePercent = 15; 
        [SerializeField] 
        private int _EpicPercent = 5; 
        [SerializeField]
        private int _CommonPackAmount; 
        [SerializeField]
        private int _RarePackAmount; 
        [SerializeField]
        private int _EpicPackAmount; 
        [SerializeField]
        private int _CommonPrice; 
        [SerializeField]
        private int _RarePrice; 
        [SerializeField] 
        private int _EpicPrice; 

        private int RandomNum;

        void Start()
        {
            ResetFunctionPakage_UI();
            SetFunctionPakage_UI();
            _allBadgeInfos.SetInfosGrade(); 
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
        /// 이벤트 세팅
        /// </summary>
        private void SetFunctionPakage_UI()
        {
            _BadgeButton.onClick.AddListener(() => Summons(1,10));
            _BadgeSackButton.onClick.AddListener(() => Summons(5, 20));
            _BadgeBoxButton.onClick.AddListener(() => Summons(11, 40));

            SetBadgeName();
        }

        private void SetBadgeName()
        {
            //BadgeCommonList.Add("튼튼한재질");
            //BadgeCommonList.Add("특대형");
            //BadgeCommonList.Add("ㅈ↑퍼");
            //BadgeCommonList.Add("ㅈ↓퍼");
            //BadgeCommonList.Add("단 1");

            //BadgeRareList.Add("바겐세일");
            //BadgeRareList.Add("이불밖은위험해");
            //BadgeRareList.Add("가시천");

            //BadgeEpicList.Add("N극");
            //BadgeEpicList.Add("S극");
            //BadgeEpicList.Add("군것질");
            //BadgeEpicList.Add("다마고치");
            //BadgeEpicList.Add("씨앗");

        }

        //private void BadgeButtonClick()
        //{
        //    /*if(현재 가진 달고나 <  _useDalgona)
        //    {
        //        return;
        //    }*/
        //    Summons(_CommonPackAmount);
        //}

        //private void BadgeSackButtonClick()
        //{
        //    /*if(현재 가진 달고나 <  _useDalgona)
        //    {
        //        return;
        //    }*/
        //    Summons(_RarePackAmount);
        //}

        //private void BadgeBoxButtonClick()
        //{
        //    /*if(현재 가진 달고나 <  _useDalgona)
        //    {
        //        return;
        //    }*/
        //    Summons(_EpicPackAmount);
        //}

        private void Summons(int Amount, int cost)
        {
            /*
             * if(현재 가진 달고나 < cost)
             * {
             *      return; 
             * }
             */
            for (int i = 0; i < Amount; i++)
            {
                BadgeSummons();
            }
        }

        // 일반 80% 희귀 15% 영웅 5%
        [ContextMenu("테스트")]
        private void BadgeSummons()
        {
            int Percent = Random.Range(0, 100 + 1);
            Debug.Log("Percent : " + Percent);
            GameObject badge = Instantiate(badgePrefab, gachaCanvas.transform);
            if (_EpicPercent >= Percent)
            {
                //에픽 스티커 소환
                RandomNum = Random.Range(0, _allBadgeInfos.badgeEpicInfos.Count);
                Debug.Log($"\"영웅\"등급 {_allBadgeInfos.badgeEpicInfos[RandomNum]} 뱃지가 나왔습니다.");
            }
            else if (_RarePercent + _EpicPercent >= Percent)
            {
                //레어 스티커 소환
                RandomNum = Random.Range(0, _allBadgeInfos.badgeRareInfos.Count);
                Debug.Log($"\"레어\"등급 {_allBadgeInfos.badgeRareInfos[RandomNum]} 뱃지가 나왔습니다.");
            }
            else
            {
                //일반 스티커 소환
                RandomNum = Random.Range(0, _allBadgeInfos.badgeCommonInfos.Count);
                Debug.Log($"\"일반\"등급 {_allBadgeInfos.badgeCommonInfos[RandomNum]} 뱃지가 나왔습니다.");
            }
            DailyItemInfo getItemInfo = _allBadgeInfos.badgeCommonInfos[RandomNum];
            ShowBadge(badge,getItemInfo); 
            //if(_EpicPercent >= Percent)
            //{
            //    //에픽 스티커 소환
            //    RandomNum = Random.Range(0, BadgeEpicList.Count);
            //    Debug.Log($"\"영웅\"등급 {BadgeEpicList[RandomNum]} 뱃지가 나왔습니다.");
            //    return;
            //}
            //if (_RarePercent >= Percent)
            //{
            //    //레어 스티커 소환
            //    RandomNum = Random.Range(0, BadgeRareList.Count);
            //    Debug.Log($"\"레어\"등급 {BadgeRareList[RandomNum]} 뱃지가 나왔습니다.");
            //    return;
            //}
            ////일반 스티커 소환
            //RandomNum = Random.Range(0, BadgeCommonList.Count);
            //Debug.Log($"\"일반\"등급 {BadgeCommonList[RandomNum]} 뱃지가 나왔습니다.");
        }

        private void ShowBadge(GameObject badge, DailyItemInfo getItemInfo)
        {
            //badgePrefab.GetComponent<Image>().sprite = getItemInfo._itemSprite;
            //badgePrefab.GetComponent<TextMeshProUGUI>().text = getItemInfo._cardName;

            GameObject badgeObj = badgePrefab.transform.Find("BadgeImage").gameObject;
            Image badgeImage = badgeObj.GetComponent<Image>();

            Sequence sequence = DOTween.Sequence();
            //sequence.AppendCallback(() =>
            //{
            //    if (badgeImage.transform.rotation.y >= 90 && badgeImage.transform.rotation.y < 270)
            //    { 
            //        badgeImage.sprite = _backBadgeImage;
            //    }
            //    else
            //    {
            //        badgeImage.sprite = getItemInfo._itemSprite;
            //    }
            //}).SetLoops(-1);
            //sequence.Append(badgeImage.transform.DORotate(Vector3.up * 360, 0.5f, RotateMode.FastBeyond360).SetLoops(10, LoopType.Yoyo));

            badgeObj.transform.DORotate(new Vector3(0,360,0), 1).SetLoops(10, LoopType.Yoyo);
        }

    }
}
