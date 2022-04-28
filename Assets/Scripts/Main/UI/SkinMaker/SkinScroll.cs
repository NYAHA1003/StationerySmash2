using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Main.Scroll;

namespace Main.Skin
{
    public class SkinScroll : AgentScroll
    {
        //인스펙터 변수
        [SerializeField]
        private GameObject _stressIconParent; // 강조 아이콘 부모

        //변수
        private List<Image> _stressIcons = new List<Image>(); //강조 아이콘 리스트
        private Vector3 _orginIconScale = Vector3.one; //원래 아이콘의 크기


        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (_curPos == _targetPos)
            {
                DeltaSlide(eventData.delta.y);
            }
            SetStressIconScale();
        }

        protected override void SettingAwake()
        {
            base.SettingAwake();
        }
        protected override void SettingStart()
        {
            base.SettingStart();
            SetStressIconList();
            SetStressIconScale();
        }

        /// <summary>
        /// 아이콘들을 리스트에 넣는다
        /// </summary>
        private void SetStressIconList()
        {
            for (int i = 0; i < Size; i++)
            {
                _stressIcons.Add(_stressIconParent.transform.GetChild(i).GetComponent<Image>());
            }
        }

        /// <summary>
        /// 아이콘들의 크기 설정
        /// </summary>
        private void SetStressIconScale()
        {
            for (int i = 0; i < _stressIcons.Count; i++)
            {
                if (_targetIndex == i)
                {
                    _stressIcons[i].transform.localScale = _orginIconScale * 1.5f;
                    _stressIcons[i].color = Color.yellow;
                }
                else
                {
                    _stressIcons[i].transform.localScale = _orginIconScale;
                    _stressIcons[i].color = Color.white;
                }
            }
        }
    }
}