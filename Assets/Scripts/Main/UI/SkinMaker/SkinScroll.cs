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
        //�ν����� ����
        [SerializeField]
        private GameObject _stressIconParent; // ���� ������ �θ�

        //����
        private List<Image> _stressIcons = new List<Image>(); //���� ������ ����Ʈ
        private Vector3 _orginIconScale = Vector3.one; //���� �������� ũ��


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
        /// �����ܵ��� ����Ʈ�� �ִ´�
        /// </summary>
        private void SetStressIconList()
        {
            for (int i = 0; i < Size; i++)
            {
                _stressIcons.Add(_stressIconParent.transform.GetChild(i).GetComponent<Image>());
            }
        }

        /// <summary>
        /// �����ܵ��� ũ�� ����
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