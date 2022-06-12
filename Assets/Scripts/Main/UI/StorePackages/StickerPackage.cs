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
        private GameObject _canvas;
        [SerializeField]
        private GameObject _gachaCanvas;
        [SerializeField]
        private GameObject _stickerPanel; 

        [SerializeField]
        private Transform _itemParent; // �⺻ canvas 
        [SerializeField]
        private Transform _gachaCanvasItemParent; // gachaCanvas 
        [SerializeField]
        private GachaCard _stickerPrefab;
        [SerializeField]
        private Sprite _backStickerImage;

        [SerializeField]
        private GachaSO _gachaSO; // ����,���� ��ƼĿ��, �� ��ƼĿ �� 

        [SerializeField]
        private AllItemInfos allItemInfos; 

        private List<int> _emptyList = new List<int>(); // ���� ������Ʈ �ε��� ���� (���� �� �޾ƿð�)
        private List<Grade> _stickerGrades = new List<Grade>(); // ��ƼĿ ���  (���� �� �޾ƿð�)

        private int _gradeIdx = 0;  // ��� �ε��� 
        private int _emptyIdx = 0; // �� ������Ʈ �ε��� 

        private void Start()
        {
            ListenEvent();
            InitializeStciker(); 
            allItemInfos.SetInfosGrade();
            InstantiateItem();
        }

        private void ListenEvent()
        {
            EventManager.Instance.StartListening(EventsType.DrawSticker, (x) => DrawSticker((GachaCard)x));
        }
        private void InstantiateItem()
        {
            for(int i = 0; i < _gachaSO.maxAmount; i++)
            {
                GachaCard sticker;
                if (_emptyList.Count != 0 && i == _emptyList[_emptyIdx])
                {
                    sticker = Instantiate(_stickerPrefab, _itemParent);
                    sticker.GetComponent<Image>().enabled = false; 
                    _emptyIdx++;
                    continue; 
                }
                sticker = Instantiate(_stickerPrefab, _itemParent);
                
                sticker.SetGrade(_stickerGrades[i]);
                sticker.SetSprite(null, _backStickerImage, false);
                //sticker.SetSprite();
                // �ʱ�ȭ �Լ� sticker.Init(Grade)
                _gradeIdx++;
            }
            Transform gachaCanvasParent = Instantiate(_itemParent, _gachaCanvasItemParent);
            gachaCanvasParent.GetChild(gachaCanvasParent.childCount - 1).gameObject.SetActive(false); // ��ư �������� ������ �г� false
        }
        
        private void DrawSticker(GachaCard stickerItem)
        {
            int randomIndex;
            DailyItemInfo getItemInfo = null; 
            switch (stickerItem._grade)
            {
                case Grade.Common:
                    randomIndex = Random.Range(0, allItemInfos.commonItemInfos.Count);
                    getItemInfo = allItemInfos.commonItemInfos[randomIndex];
                    break;
                case Grade.Rare:
                    randomIndex = Random.Range(0, allItemInfos.rareItemInfos.Count);
                    getItemInfo = allItemInfos.rareItemInfos[randomIndex];
                    break;
                case Grade.Epic:
                    randomIndex = Random.Range(0, allItemInfos.epicItemInfos.Count);
                    getItemInfo = allItemInfos.epicItemInfos[randomIndex];
                    break;
            }
            stickerItem.SetSprite(getItemInfo._itemSprite,_backStickerImage, false);
            stickerItem.ActiveAndAnimate(); 
        }
        /// <summary>
        /// ��ƼĿ�� �ʱ�ȭ 
        /// ��ƼĿ�� ����� ��ư�� �ֱ� 
        /// </summary>
        private void InitializeStciker()
        {
            _stickerGrades.Clear(); 
            for (int i = 0; i < _gachaSO.epicCount; i++)
            {
                _stickerGrades.Add(Grade.Epic);
            }
            for(int i = 0; i < _gachaSO.rareCount; i++)
            {
                _stickerGrades.Add(Grade.Rare);
            }
            while(_stickerGrades.Count < 70)
            {
                _stickerGrades.Add(Grade.Common);
            }
            Shuffle();
        }
        private void Shuffle()
        {
            for (int i = 0; i < 500; i++)
            {
                int idx1, idx2;
                Grade temp;
                idx1 = Random.Range(0, _gachaSO.maxAmount);
                idx2 = Random.Range(0, _gachaSO.maxAmount);

                temp = _stickerGrades[idx1];
                _stickerGrades[idx1] = _stickerGrades[idx2];
                _stickerGrades[idx2] = temp;
            }
        }
        private void SummonItem(int amount)
        {
            
        }
        /// <summary>
        /// �������� ����Ʈ ����
        /// </summary>
        private void SortList()
        {
            _emptyList.Sort();
        }

    }
}