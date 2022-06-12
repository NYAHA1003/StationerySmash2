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
        private Transform _itemParent; // 기본 canvas 
        [SerializeField]
        private Transform _canvasParent; // gachaCanvas 
        [SerializeField]
        private GachaCard _stickerPrefab;

        [SerializeField]
        private int _epicCount; // 영웅스터커 수 (받아올거)
        [SerializeField]
        private int _rareCount; // 레어 스티커 수 (받아올거) 

        [SerializeField]
        private AllItemInfos allItemInfos; 

        private List<int> _emptyList; // 뽑은 오브젝트 인덱스 저장 (자장 후 받아올거)
        private List<Grade> _stickerGrades; // 스티커 등급  (자장 후 받아올거)

        private int _gradeIdx = 0;  // 등급 인덱스 
        private int _emptyIdx = 0; // 빈 오브젝트 인덱스 
        private int _amount; // 전체 스티커 개수 
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
                // 초기화 함수 sticker.Init(Grade)
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
        /// 스티커판 초기화 
        /// 스티커판 만들기 버튼에 넣기 
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
        /// 오름차운 리스트 정렬
        /// </summary>
        private void SortList()
        {
            _emptyList.Sort();
        }

    }
}