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
            _BadgeButton.onClick.AddListener(BadgeButtonClick);
            _BadgeSackButton.onClick.AddListener(BadgeSackButtonClick);
            _BadgeBoxButton.onClick.AddListener(BadgeBoxButtonClick);

            SetBadgeName();
        }

        private void SetBadgeName()
        {
            BadgeCommonList.Add("ưư������");
            BadgeCommonList.Add("Ư����");
            BadgeCommonList.Add("������");
            BadgeCommonList.Add("������");
            BadgeCommonList.Add("�� 1");

            BadgeRareList.Add("�ٰռ���");
            BadgeRareList.Add("�̺ҹ���������");
            BadgeRareList.Add("����õ");

            BadgeEpicList.Add("N��");
            BadgeEpicList.Add("S��");
            BadgeEpicList.Add("������");
            BadgeEpicList.Add("�ٸ���ġ");
            BadgeEpicList.Add("����");

        }

        private void BadgeButtonClick()
        {
            /*if(���� ���� �ް� <  _useDalgona)
            {
                return;
            }*/
            Summons(_CommonPackAmount);
        }

        private void BadgeSackButtonClick()
        {
            /*if(���� ���� �ް� <  _useDalgona)
            {
                return;
            }*/
            Summons(_RarePackAmount);
        }

        private void BadgeBoxButtonClick()
        {
            /*if(���� ���� �ް� <  _useDalgona)
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
                //���� ��ƼĿ ��ȯ
                RandomNum = Random.Range(0, BadgeEpicList.Count);
                Debug.Log($"\"����\"��� {BadgeEpicList[RandomNum]} ������ ���Խ��ϴ�.");
                return;
            }
            if (_RarePercent >= Percent)
            {
                //���� ��ƼĿ ��ȯ
                RandomNum = Random.Range(0, BadgeRareList.Count);
                Debug.Log($"\"����\"��� {BadgeRareList[RandomNum]} ������ ���Խ��ϴ�.");
                return;
            }
            //�Ϲ� ��ƼĿ ��ȯ
            RandomNum = Random.Range(0, BadgeEpicList.Count);
            Debug.Log($"\"�Ϲ�\"��� {BadgeCommonList[RandomNum]} ������ ���Խ��ϴ�.");
        }
    }
}