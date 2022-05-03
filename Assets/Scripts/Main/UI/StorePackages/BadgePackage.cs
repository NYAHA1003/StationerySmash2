using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Data;

namespace Main.Store
{
    public class BadgePackage : MonoBehaviour
    {
        private List<string> BadgeCommonList = new List<string>();
        private List<string> BadgeRareList = new List<string>();
        private List<string> BadgeEpicList = new List<string>();

        [SerializeField]
        Button _BadgeButton;
        [SerializeField]
        Button _BadgeSackButton;
        [SerializeField]
        Button _BadgeBoxButton;

        [SerializeField]
        private int _RarePercent;
        [SerializeField]
        private int _EpicPercent;
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
            _BadgeButton.onClick.AddListener(BadgeButtonClick);
            _BadgeSackButton.onClick.AddListener(BadgeSackButtonClick);
            _BadgeBoxButton.onClick.AddListener(BadgeBoxButtonClick);

            SetBadgeName();
        }

        private void SetBadgeName()
        {
            BadgeCommonList.Add("튼튼한재질");
            BadgeCommonList.Add("특대형");
            BadgeCommonList.Add("ㅈ↑퍼");
            BadgeCommonList.Add("ㅈ↓퍼");
            BadgeCommonList.Add("단 1");

            BadgeRareList.Add("바겐세일");
            BadgeRareList.Add("이불밖은위험해");
            BadgeRareList.Add("가시천");

            BadgeEpicList.Add("N극");
            BadgeEpicList.Add("S극");
            BadgeEpicList.Add("군것질");
            BadgeEpicList.Add("다마고치");
            BadgeEpicList.Add("씨앗");

        }

        private void BadgeButtonClick()
        {
            /*if(현재 가진 달고나 <  _useDalgona)
            {
                return;
            }*/
            Summons(_CommonPackAmount);
        }

        private void BadgeSackButtonClick()
        {
            /*if(현재 가진 달고나 <  _useDalgona)
            {
                return;
            }*/
            Summons(_RarePackAmount);
        }

        private void BadgeBoxButtonClick()
        {
            /*if(현재 가진 달고나 <  _useDalgona)
            {
                return;
            }*/
            Summons(_EpicPackAmount);
        }

        private void Summons(int Amount)
        {
            for (int i = 0; i < Amount; i++)
            {
                BadgeSummons();
            }
        }

        private void BadgeSummons()
        {
            int Percent = Random.Range(0, 100 + 1);
            if(_EpicPercent >= Percent)
            {
                //에픽 스티커 소환
                RandomNum = Random.Range(0, BadgeEpicList.Count);
                Debug.Log($"\"영웅\"등급 {BadgeEpicList[RandomNum]} 뱃지가 나왔습니다.");
                return;
            }
            if (_RarePercent >= Percent)
            {
                //레어 스티커 소환
                RandomNum = Random.Range(0, BadgeRareList.Count);
                Debug.Log($"\"레어\"등급 {BadgeRareList[RandomNum]} 뱃지가 나왔습니다.");
                return;
            }
            //일반 스티커 소환
            RandomNum = Random.Range(0, BadgeEpicList.Count);
            Debug.Log($"\"일반\"등급 {BadgeCommonList[RandomNum]} 뱃지가 나왔습니다.");
        }
    }
}