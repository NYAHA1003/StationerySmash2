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
//                    //���� ��ƼĿ ��ȯ
//                    RandomNum = Random.Range(0, allBadgeInfos.epicItemInfos.Count);
//                    Debug.Log($"\"����\"��� {allBadgeInfos.epicItemInfos[RandomNum]} ������ ���Խ��ϴ�.");
//                }
//                else if (rarePercent + epicPercent >= Percent)
//                {
//                    //���� ��ƼĿ ��ȯ
//                    RandomNum = Random.Range(0, allBadgeInfos.rareItemInfos.Count);
//                    Debug.Log($"\"����\"��� {allBadgeInfos.rareItemInfos[RandomNum]} ������ ���Խ��ϴ�.");
//                }
//                else
//                {
//                    //�Ϲ� ��ƼĿ ��ȯ
//                    RandomNum = Random.Range(0, allBadgeInfos.commonItemInfos.Count);
//                    Debug.Log($"\"�Ϲ�\"��� {allBadgeInfos.commonItemInfos[RandomNum]} ������ ���Խ��ϴ�.");
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