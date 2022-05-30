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
            if (badgeInfo._grade == Grade.Common)
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
        private Canvas gachaCanvas;
        [SerializeField]
        private GameObject itemsParent;
        [SerializeField]
        private GachaCard badgePrefab;
        [SerializeField]
        private Image blackBackImage;

        private int _count = 11; // �ִ� ������ �� ����
        //private List<string> BadgeCommonList = new List<string>();
        //private List<string> BadgeRareList = new List<string>();
        //private List<string> BadgeEpicList = new List<string>();

        [SerializeField]
        Button _BadgeButton; // 1��
        [SerializeField]
        Button _BadgeSackButton; // 5��
        [SerializeField]
        Button _BadgeBoxButton; // 11�� 

        [SerializeField]
        private Sprite _backBadgeImage; // ���� �޸� 

        private List<GachaCard> gachaCards = new List<GachaCard>();
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
            InstantiateItem();
        }

        /// <summary>
        /// �̺�Ʈ �ʱ�ȭ
        /// </summary>
        private void ResetFunctionPakage_UI()
        {
            _BadgeButton.onClick.RemoveAllListeners();
            _BadgeSackButton.onClick.RemoveAllListeners();
            _BadgeBoxButton.onClick.RemoveAllListeners();
        }

        /// <summary>
        /// �̺�Ʈ ����
        /// </summary>
        private void SetFunctionPakage_UI()
        {
            _BadgeButton.onClick.AddListener(() => Summons(1, 10));
            _BadgeSackButton.onClick.AddListener(() => Summons(5, 20));
            _BadgeBoxButton.onClick.AddListener(() => Summons(11, 40));

            SetBadgeName();
        }

        private void SetBadgeName()
        {
            //BadgeCommonList.Add("ưư������");
            //BadgeCommonList.Add("Ư����");
            //BadgeCommonList.Add("������");
            //BadgeCommonList.Add("������");
            //BadgeCommonList.Add("�� 1");

            //BadgeRareList.Add("�ٰռ���");
            //BadgeRareList.Add("�̺ҹ���������");
            //BadgeRareList.Add("����õ");

            //BadgeEpicList.Add("N��");
            //BadgeEpicList.Add("S��");
            //BadgeEpicList.Add("������");
            //BadgeEpicList.Add("�ٸ���ġ");
            //BadgeEpicList.Add("����");

        }

        //private void BadgeButtonClick()
        //{
        //    /*if(���� ���� �ް� <  _useDalgona)
        //    {
        //        return;
        //    }*/
        //    Summons(_CommonPackAmount);
        //}

        //private void BadgeSackButtonClick()
        //{
        //    /*if(���� ���� �ް� <  _useDalgona)
        //    {
        //        return;
        //    }*/
        //    Summons(_RarePackAmount);
        //}

        //private void BadgeBoxButtonClick()
        //{
        //    /*if(���� ���� �ް� <  _useDalgona)
        //    {
        //        return;
        //    }*/
        //    Summons(_EpicPackAmount);
        //}

        private void Summons(int Amount, int cost)
        {
            /*
             * if(���� ���� �ް� < cost)
             * {
             *      return; 
             * }
             */
            BadgeSummons(Amount);
        }

        // �Ϲ� 80% ��� 15% ���� 5%
        [ContextMenu("�׽�Ʈ")]
        private void BadgeSummons(int amount)
        {
            blackBackImage.gameObject.SetActive(true);
            for (int i = 0; i< amount; i++)
            {
                int Percent = Random.Range(0, 100 + 1);
                Debug.Log("Percent : " + Percent);
                if (_EpicPercent >= Percent)
                {
                    //���� ��ƼĿ ��ȯ
                    RandomNum = Random.Range(0, _allBadgeInfos.badgeEpicInfos.Count);
                    Debug.Log($"\"����\"��� {_allBadgeInfos.badgeEpicInfos[RandomNum]} ������ ���Խ��ϴ�.");
                }
                else if (_RarePercent + _EpicPercent >= Percent)
                {
                    //���� ��ƼĿ ��ȯ
                    RandomNum = Random.Range(0, _allBadgeInfos.badgeRareInfos.Count);
                    Debug.Log($"\"����\"��� {_allBadgeInfos.badgeRareInfos[RandomNum]} ������ ���Խ��ϴ�.");
                }
                else
                {
                    //�Ϲ� ��ƼĿ ��ȯ
                    RandomNum = Random.Range(0, _allBadgeInfos.badgeCommonInfos.Count);
                    Debug.Log($"\"�Ϲ�\"��� {_allBadgeInfos.badgeCommonInfos[RandomNum]} ������ ���Խ��ϴ�.");
                }
                DailyItemInfo getItemInfo = _allBadgeInfos.badgeCommonInfos[RandomNum];
                gachaCards[i].gameObject.SetActive(true); 
                gachaCards[i].SetSprite(getItemInfo._itemSprite, _backBadgeImage);
            }
            
            //if(_EpicPercent >= Percent)
            //{
            //    //���� ��ƼĿ ��ȯ
            //    RandomNum = Random.Range(0, BadgeEpicList.Count);
            //    Debug.Log($"\"����\"��� {BadgeEpicList[RandomNum]} ������ ���Խ��ϴ�.");
            //    return;
            //}
            //if (_RarePercent >= Percent)
            //{
            //    //���� ��ƼĿ ��ȯ
            //    RandomNum = Random.Range(0, BadgeRareList.Count);
            //    Debug.Log($"\"����\"��� {BadgeRareList[RandomNum]} ������ ���Խ��ϴ�.");
            //    return;
            //}
            ////�Ϲ� ��ƼĿ ��ȯ
            //RandomNum = Random.Range(0, BadgeCommonList.Count);
            //Debug.Log($"\"�Ϲ�\"��� {BadgeCommonList[RandomNum]} ������ ���Խ��ϴ�.");
        }

        /// <summary>
        ///  �̱� �ݱ� 
        /// </summary>
        private void Close()
        {
            for(int i =0; i < _count; i++)
            {
                gachaCards[i].gameObject.SetActive(true);
            }
            // ���� �̹��� �ݱ� 
        }
        /// <summary>
        /// �ִ�� �� ������ ������ŭ �̸� �����صα� 
        /// </summary>
        private void InstantiateItem()
        {
            for (int i = 0; i < _count; ++i)
            {
                GachaCard gachaCard = Instantiate(badgePrefab, itemsParent.transform);
                gachaCards.Add(gachaCard);
            }
        }

    }

}
