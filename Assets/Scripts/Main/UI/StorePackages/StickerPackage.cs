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
        private Transform _itemParent; // �⺻ canvas 
        [SerializeField]
        private Transform _canvasParent; // gachaCanvas 
        [SerializeField]
        private GachaCard _stickerPrefab;

        [SerializeField]
        private int _epicCount; // ��������Ŀ �� (�޾ƿð�)
        [SerializeField]
        private int _rareCount; // ���� ��ƼĿ �� (�޾ƿð�) 

        [SerializeField]
        private AllItemInfos allItemInfos; 

        private List<int> _emptyList; // ���� ������Ʈ �ε��� ���� (���� �� �޾ƿð�)
        private List<Grade> _stickerGrades; // ��ƼĿ ���  (���� �� �޾ƿð�)

        private int _gradeIdx = 0;  // ��� �ε��� 
        private int _emptyIdx = 0; // �� ������Ʈ �ε��� 
        private int _amount; // ��ü ��ƼĿ ���� 
        private void Start()
        {
            allItemInfos.SetInfosGrade();
            InstantiateItem();
        }

        private void InstantiateItem()
        {
            for(int i = 0; i < _amount; i++)
            {
                if (_amount == _emptyList[_emptyIdx])
                {
                    Instantiate(new GameObject(), _itemParent);
                    _emptyIdx++; 
                }
                GachaCard sticker = Instantiate(_stickerPrefab, _itemParent);
                // �ʱ�ȭ �Լ� sticker.Init(Grade)
                _gradeIdx++;
            }
            Instantiate(_itemParent, _canvasParent);
        }

        private void DrawSticker(Grade grade)
        {
            int randomIndex; 
            switch (grade)
            {
                case Grade.Common:
                    randomIndex = Random.Range(0, allItemInfos.commonItemInfos.Count);
                    break;
                case Grade.Rare:
                    randomIndex = Random.Range(0, allItemInfos.rareItemInfos.Count);
                    break;
                case Grade.Epic:
                    randomIndex = Random.Range(0, allItemInfos.epicItemInfos.Count);
                    break;
            }

        }
        /// <summary>
        /// ��ƼĿ�� �ʱ�ȭ 
        /// ��ƼĿ�� ����� ��ư�� �ֱ� 
        /// </summary>
        private void InitializeStciker()
        {
            _stickerGrades.Clear(); 
            for (int i = 0; i < _epicCount; i++)
            {
                _stickerGrades.Add(Grade.Epic);
            }
            for(int i = 0; i < _rareCount; i++)
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
                idx1 = Random.Range(0, _amount);
                idx2 = Random.Range(0, _amount);

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