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
    public class StickerPackage : MonoBehaviour 
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

        private int _count = 11; // �ִ� ������ �� ����

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

        private int currentNum; // ���� ���° ������ ������ 
        private int currentAmount; // ���� �� ���� ������ �� 

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
        /// �̺�Ʈ �ʱ�ȭ
        /// </summary>
        private void ResetFunctionPakage_UI()
        {
            _BadgeButton.onClick.RemoveAllListeners();
            _BadgeSackButton.onClick.RemoveAllListeners();
            _BadgeBoxButton.onClick.RemoveAllListeners();
        }

        /// <summary>
        /// �̺�Ʈ�Ŵ����� �̺�Ʈ ��� 
        /// </summary>
        private void ListenEvent()
        {
            EventManager.Instance.StartListening(EventsType.CloseGacha, Close);
            EventManager.Instance.StartListening(EventsType.CheckItem, CheckItem);
        }
        /// <summary>
        /// �̺�Ʈ ����
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
             * if(���� ���� �ް� < cost)
             * {
             *      return; 
             * }
             */
            InitGacha(Amount);
            BadgeSummons();
        }

        // �Ϲ� 80% ��� 15% ���� 5%
        [ContextMenu("�׽�Ʈ")]
        private void BadgeSummons()
        {
            blackBackImage.gameObject.SetActive(true);
            for (int i = 0; i < currentAmount; i++)
            {
                int Percent = Random.Range(0, 100 + 1);
                Debug.Log("Percent : " + Percent);
                if (_EpicPercent >= Percent)
                {
                    //���� ��ƼĿ ��ȯ
                    RandomNum = Random.Range(0, _allBadgeInfos.epicItemInfos.Count);
                    Debug.Log($"\"����\"��� {_allBadgeInfos.epicItemInfos[RandomNum]} ������ ���Խ��ϴ�.");
                }
                else if (_RarePercent + _EpicPercent >= Percent)
                {
                    //���� ��ƼĿ ��ȯ
                    RandomNum = Random.Range(0, _allBadgeInfos.rareItemInfos.Count);
                    Debug.Log($"\"����\"��� {_allBadgeInfos.rareItemInfos[RandomNum]} ������ ���Խ��ϴ�.");
                }
                else
                {
                    //�Ϲ� ��ƼĿ ��ȯ
                    RandomNum = Random.Range(0, _allBadgeInfos.commonItemInfos.Count);
                    Debug.Log($"\"�Ϲ�\"��� {_allBadgeInfos.commonItemInfos[RandomNum]} ������ ���Խ��ϴ�.");
                }
                DailyItemInfo getItemInfo = _allBadgeInfos.commonItemInfos[RandomNum];
                gachaCards[i].ActiveAndAnimate();
                gachaCards[i].SetSprite(getItemInfo._itemSprite, _backBadgeImage);
            }

        }

        /// <summary>
        /// ī�� ���徿 �� �� �ֵ��� 
        /// </summary>
        private void CheckItem()
        {
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
            if (amount == 1)
            {
                nextBtn.SetActive(false);
                return;
            }
            nextBtn.SetActive(true);
        }
        /// <summary>
        ///  �̱� �ݱ� 
        /// </summary>
        private void Close()
        {
            for (int i = 0; i < _count; i++)
            {
                gachaCards[i].gameObject.SetActive(false);
            }
            // ���� �̹��� �ݱ� 
            blackBackImage.gameObject.SetActive(false);
        }
        /// <summary>
        /// �ִ�� �� ������ ������ŭ �̸� �����صα� 
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