//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Main.Store;

//namespace Main.Store
//{
//    public class BadgeGacha2 : AbstractGacha
//    {
//        [SerializeField]
//        private GameObject nextBtn;
//        public override void CheckItem()
//        {
//            if (currentNum == currentAmount - 1)
//            {
//                nextBtn.SetActive(false);
//                return;
//            }
//            for (int i = 0; i < currentAmount; i++)
//            {
//                gachaCards[i].gameObject.SetActive(false);
//            }
//            gachaCards[++currentNum].StressOneItem();
//        }   

//        public override void ItemSummons()
//        {
//            blackBackImage.gameObject.SetActive(true);
//            AllBadgeInfos allBadgeInfos = gachaInfo.allBadgeInfos;
//            float epicPercent = gachaInfo.gachaSO.epicPercent;
//            float rarePercent = gachaInfo.gachaSO.rarePercent;

//            for (int i = 0; i < currentAmount; i++)
//            {
//                int Percent = Random.Range(0, 100 + 1);
//                Debug.Log("Percent : " + Percent);
//                if (epicPercent >= Percent)
//                {
//                    //에픽 스티커 소환
//                    RandomNum = Random.Range(0, allBadgeInfos.epicItemInfos.Count);
//                    Debug.Log($"\"영웅\"등급 {allBadgeInfos.epicItemInfos[RandomNum]} 뱃지가 나왔습니다.");
//                }
//                else if (rarePercent + epicPercent >= Percent)
//                {
//                    //레어 스티커 소환
//                    RandomNum = Random.Range(0, allBadgeInfos.rareItemInfos.Count);
//                    Debug.Log($"\"레어\"등급 {allBadgeInfos.rareItemInfos[RandomNum]} 뱃지가 나왔습니다.");
//                }
//                else
//                {
//                    //일반 스티커 소환
//                    RandomNum = Random.Range(0, allBadgeInfos.commonItemInfos.Count);
//                    Debug.Log($"\"일반\"등급 {allBadgeInfos.commonItemInfos[RandomNum]} 뱃지가 나왔습니다.");
//                }
//                DailyItemInfo getItemInfo = allBadgeInfos.commonItemInfos[RandomNum];
//                gachaCards[i].ActiveAndAnimate();
//                gachaCards[i].SetSprite(getItemInfo._itemSprite, _backBadgeImage,isFront: true);
//            }
//        }
//        private void InitGacha(int amount)
//        {
//            currentAmount = amount;
//            currentNum = 0;
//            nextBtn.SetActive(true);
//        }

//        public override void Summon()
//        {
//            if(CheckCost() == true)
//            InitGacha(amount);
//            ItemSummons();
//        }

//    }
//}